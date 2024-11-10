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
    
    <div v-for="(hardware, index) in hardwareOptions" :key="hardware.id">
      <label>
        <input type="checkbox"
               :value="hardware.id"
               v-model="selectedHardware" />
        {{ hardware.name }}  ({{ hardware.totalAmount }} verfügbar)
        <input type="number"
               v-model.number="quantity[hardware.id]"
               :min="0"
               :max="hardware.totalAmount"
               v-if="selectedHardware.includes(hardware.id)" />
      </label>
    </div>
    <button type="submit">Reservieren</button>
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
  import { ref, onMounted } from "vue";

  export default {
  name: 'HardwareReservation',
    setup(){
      const hardwareOptions = ref([]);
      const quantity = ref({});
      const hardwareId = ref({});
      const reservationRequest = {};
      const selectedEvent = ref({});
      const eventsForReservation = ref([]);
      const selectedHardware = ref([]);
      const successMsg = ref('');
      const errorMsg = ref('');
      
      const fetchHardwareList = async () => {
        try {
          
          const [res1, res2] = await Promise.all([
            fetch('hardware'),  // Daten von Hardware holen
            fetch('/hardware/eventsForReservation')   // Daten von fiktive Events für die Reservation
          ]);

          hardwareOptions.value = await res1.json(); // Daten in Json umwandeln
          const infoEvents = await res2.json();

          infoEvents.forEach((eventsInfo) => {
            const startDate = new Date(eventsInfo.eventStartDate);
            const endDate = new Date(eventsInfo.eventEndDate);

            eventsInfo.eventStartDate = startDate.toLocaleDateString('de-DE');
            eventsInfo.eventEndDate = endDate.toLocaleDateString('de-DE');
          });

          eventsForReservation.value = infoEvents;

          if (hardwareOptions.value != null) {
            hardwareOptions.value.forEach((hardware) => {
              quantity.value[hardware.id] = 0; // Jeder Anzahl mit 0 inisialisieren
            });
          }
        } catch (error){
          console.error('Error fetching hardware data:', error);
        }
      };

      const reserveHardware = async () => {
        if (selectedHardware.value.length > 0) {
          const hardwareForReservation = selectedHardware.value
            .filter(hardwareId => quantity.value[hardwareId] > 0)  // Solo incluye el hardware con cantidad > 0
            .map(hardwareId => ({
              hardwareId: hardwareId,
              quantity: quantity.value[hardwareId]  // Asocia la cantidad seleccionada con el hardwareId
            }));

          // Verificar si la lista de hardware con cantidades es válida
          if (hardwareForReservation.length > 0) {
            try {
              const reservationRequest = {
                eventId: selectedEvent.value.eventId,
                hardwareList: hardwareForReservation  // Envía la lista de hardware con cantidades
              };

              const response = await fetch("/reservation", {
                method: "POST",
                headers: {
                  "Content-Type": "application/json",
                },
                body: JSON.stringify(reservationRequest),
              });

              if (response.ok) {
                successMsg.value = "Die Reservierung wurde gemacht, eine Freigabe steht aus. Bitte aktuelle Reservierungen Seite ansehen";
                errorMsg.value = "";
              } else {
                errorMsg.value = "Die Reservierungsdatum ist zu kurzfristig. Die Reservierung darf nur mindestens 4 Woche vor Veranstaltungsdurchführung getätigt.";
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

      onMounted(fetchHardwareList);

      return {
        hardwareOptions,
        selectedHardware,
        selectedEvent,
        eventsForReservation,
        hardwareId,
        quantity,
        reservationRequest,
        reserveHardware,
        successMsg,
        errorMsg,
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

  /* Título */
  h2 {
    text-align: center;
    color: #333;
    font-size: 1.8em;
    margin-bottom: 10px;
  }

  /* Texto de advertencia */
  p {
    text-align: center;
    font-size: 1em;
    color: #d9534f;
    margin-bottom: 20px;
  }

  /* Etiquetas y campos de selección */
  label {
    display: block;
    font-size: 1em;
    color: #555;
    margin-bottom: 8px;
  }


</style>
