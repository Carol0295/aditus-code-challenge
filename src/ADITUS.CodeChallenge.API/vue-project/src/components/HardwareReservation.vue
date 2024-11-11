 <template>
  <h2>Reservierung</h2>
  <p>Achtung: Die Reservierung darf nur mindestens 4 Woche vor Veranstaltungsdurchführung getätigt werden.</p>
  <form @submit.prevent="reserveHardware">
    <div>
      <label for="myEvent">Bitte wählen Sie ein Event aus der Liste aus. </label>
      <select v-model="selectedEvent" id="myEvent" required>
        <option v-for="event in eventsForReservation" :key="event.eventId" :value="event">
          {{event.eventName}} - ( {{event.eventStartDate}} bis {{event.eventEndDate}} )
        </option>
      </select>
    </div>

    <div v-if="hardwareAvailability">
      <div v-for="hardware in hardwareOptions" :key="hardware.id">
        <label>
          <input type="checkbox" :value="hardware.id" v-model="selectedHardware" />
          {{ hardware.name }} ({{ hardwareAvailability[hardware.id] || 0 }} verfügbar)
          <input v-model="quantity[hardware.id]" :max="hardwareAvailability[hardware.id]" type="number" min="0" :disabled="hardwareAvailability[hardware.id] <= 0" />
        </label>
      </div>
      <button type="submit">Reservieren</button>
    </div>
  </form>
  <!-- Wenn erfolgreich: -->
  <div v-if="successMsg" class="success-message">
    {{ successMsg }}
  </div>
  <div v-else-if="errorMsg" class="error-message">
    {{ errorMsg }}
  </div>
</template>

<script>
  import { ref, onMounted, watch } from "vue";

  export default {
  name: 'HardwareReservation',
    setup(){
      const hardwareOptions = ref([]);
      const quantity = ref({});
      const hardwareId = ref({});
      const selectedEvent = ref(null);
      const eventsForReservation = ref([]);
      const selectedHardware = ref([]);
      const successMsg = ref('');
      const errorMsg = ref('');
      const allReservations = ref([]);
      const hardwareAvailability = ref({});


      /* Funktion zum Anzeigen der Veranstaltungen und die Hardware zum auswählen (auch die Abholung von der in Memory gespeicherte Reservierungen)*/
      const fetchHardwareList = async () => {
        try {
          
          const [res1, res2, res3] = await Promise.all([
            fetch('hardware'),  // Daten von Hardware holen
            fetch('/reservation/eventsForReservation'),   // Daten von fiktive Events für die Reservation
            fetch('/reservation/allReservations')   // Daten von fiktive Events für die Reservation
          ]);

          hardwareOptions.value = await res1.json(); // Daten in Json umwandeln
          const infoEvents = await res2.json();
          allReservations.value = await res3.json();

          infoEvents.forEach((eventsInfo) => {
            const startDate = new Date(eventsInfo.eventStartDate);
            const endDate = new Date(eventsInfo.eventEndDate);

            eventsInfo.eventStartDate = startDate.toLocaleDateString('de-DE');
            eventsInfo.eventEndDate = endDate.toLocaleDateString('de-DE');

          });

          
          eventsForReservation.value = infoEvents;

          // Menge an Hardware mit dem TotalAmount initialisieren
          hardwareOptions.value.forEach((hardware) => {
            hardwareAvailability.value[hardware.id] = hardware.totalAmount;
          });

        } catch (error){
          console.error('Error fetching hardware data:', error);
        }
      };

      /* Funktion zum Senden der Informationen über die ausgewähle Event und Hardware Ids und Mengen in dii ReservationsController. */
      const reserveHardware = async () => {
        if (selectedHardware.value.length > 0) {
          // Prüfen ob es ausgewählte Elemente gibt, wo die Anzahl größer als 0 ist und packt sie zusammen mit dem HardwareId
          const hardwareForReservation = selectedHardware.value
            .filter(hardwareId => quantity.value[hardwareId] > 0) 
            .map(hardwareId => ({
              hardwareId: hardwareId,
              quantity: quantity.value[hardwareId]
            }));

          // Prüfen ob die Liste was enthält und bereitet die Variablen zum Senden in den Post mit dem Event-Id und die HardwareList (HardwareIds und Anzahl an Hardware)
          if (hardwareForReservation.length > 0) {
            try {
              const reservationRequest = {
                eventId: selectedEvent.value.eventId,
                hardwareList: hardwareForReservation  
              };

              const response = await fetch("/reservation", {
                method: "POST",
                headers: {
                  "Content-Type": "application/json",
                },
                body: JSON.stringify(reservationRequest),
              });

              // Je nachdem, ob es erfolgreich oder nicht ist, wird eine Benachrichtigung wird hier angezeigt (in template oben).
              const result = await response.json();
              if (result.success) {
                successMsg.value = result.message;
                errorMsg.value = "";
              } else {
                errorMsg.value = result.message;
                successMsg.value = "";
              }
            } catch (error) {
              errorMsg.value = "Fehler bei der Serververbindung.";
              console.error("Error:", error);
            }
          } else {
            errorMsg.value = "Bitte wählen Sie Hardware mit Menge zur Reservierung aus.";
            successMsg.value = "";
          }
        } else {
          errorMsg.value = 'Bitte wählen Sie Hardware aus, um eine Reservierung zu machen.';
        }
      };

      /* Funktion zum prüfen, ob Hardware zur Verfügung gibt*/
      const isHardwareAvailable = () => {
        const availability = {};

        hardwareOptions.value.forEach((hardware) => {
          availability[hardware.id] = hardware.totalAmount; // Hardware initialisieren mit dem kompletten Anzahl an Hardware
        });

        // Wenn Event selektiert ist, Reservierungen in Memory durchlaufen
        if (selectedEvent.value) {
          allReservations.value.forEach((reservation) => {
            const reservationEventDate = new Date(reservation.eventStartDate);
            reservation.eventStartDate = reservationEventDate.toLocaleDateString('de-DE');

            // Wenn Veranstaltung in gleichen Tag ist, und HardwareList nicht leer ist, reservierte HardwareList durchlaufen
            if (reservation.eventStartDate === selectedEvent.value.eventStartDate && reservation.hardwareList) { 
              reservation.hardwareList.forEach((hardwareReserved) => {
                const hardwareId = hardwareReserved.hardwareId;
                const quantityReserved = hardwareReserved.quantity;
                if (availability[hardwareId] !== undefined) {
                  availability[hardwareId] = availability[hardwareId] - quantityReserved; // Menge abziehen, falls in Memory schon Hardware gibt in Reservierung in Memory um das neue totate verfügbare Anzahl von Hardware zu zeigen
                }
              });
            }
          });
        }

        // Aktualisieren von hardwareAvailability - also die Hardware zur Verfügung (Die Anzahl)
        hardwareAvailability.value = availability;
      };

      onMounted(() => {
        fetchHardwareList().then(() => {
          isHardwareAvailable();
        });
      });

      // Wenn ein anderes Veranstaltung ausgewählt wird, wird diese Funktion ausgelöst
      watch(selectedEvent, () => {
        isHardwareAvailable();
      });

      return {
        hardwareOptions,
        selectedHardware,
        selectedEvent,
        eventsForReservation,
        hardwareId,
        quantity,
        reserveHardware,
        successMsg,
        errorMsg,
        hardwareAvailability
      };
    },
  };

</script>

<style>
  form {
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
    background-color: #f9f9f9;
    border: 1px solid #ddd;
    border-radius: 8px;
  }

  h2 {
    text-align: center;
    color: #333;
    font-size: 1.8em;
    margin-bottom: 10px;
  }

  p {
    text-align: center;
    font-size: 1em;
    color: #d9534f;
    margin-bottom: 20px;
  }

  label {
    display: block;
    font-size: 1em;
    color: #555;
    margin-bottom: 8px;
  }

  select, input[type="number"] {
    margin-bottom: 15px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1em;
  }

  select:focus, input[type="number"]:focus {
    border-color: #66afe9;
    outline: none;
  }

  input[type="checkbox"] {
    margin-right: 10px;
  }

  div[v-for] label {
    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: 0.95em;
    margin-bottom: 8px;
  }

  button[type="submit"] {
    display: block;
    padding: 10px;
    background-color: #e3e9c6;
    border: none;
    border-radius: 4px;
    font-size: 1em;
    cursor: pointer;
  }

  button[type="submit"]:hover {
    background-color: darkgrey;
  }

  .success-message, .error-message {
    max-width: 600px;
    margin: 15px auto;
    padding: 10px;
    text-align: center;
    font-size: 1em;
    border-radius: 4px;
  }

  .success-message {
    background-color: #dff0d8;
    color: #3c763d;
    border: 1px solid #d6e9c6;
  }

  .error-message {
    background-color: #f2dede;
    color: #a94442;
    border: 1px solid #ebccd1;
  }
</style>
