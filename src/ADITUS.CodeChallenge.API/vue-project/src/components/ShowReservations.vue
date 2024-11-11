<template>
  <h2>Aktuelle Reservierungen</h2>
  <div class="reserveContainer">
    <ul>
      <li v-for="reservation in allReservations" :key="reservation.id" class="reservation-item">
        <strong>Freigabe ausstehend...</strong><br />
        <strong>Reservierungsnummer:</strong> {{ reservation.id }}<br />
        <strong>Für den Veranstaltung:</strong> {{ reservation.eventName }}<br />
        <strong>Reservierte Hardware:</strong>

        <ul>
          <li v-for="hardwareReserved in reservation.hardwareList" :key="hardwareId" class="reservation-item">
            {{ hardwareReserved.hardwareName }} - Menge: {{ hardwareReserved.quantity }}
          </li>
        </ul>
      </li>
    </ul>
  </div>
</template>

<script>
  import { ref, onMounted } from "vue";

  export default {
    name: "ShowReservations",
    setup() {
      const allReservations = ref([]);

      const fetchAllReservations = async () => {
        try {
          const response = await fetch('/reservation/allReservations'); // Zugriff auf Controller /reservation mit der Route allReservations. Bekommen alle Reservierungen in Memory
          
          allReservations.value = await response.json();
        } catch (error) {
          console.error('Error fetching hardware data:', error);
        }
      };

      onMounted(fetchAllReservations);

      return {
        allReservations
      };
    },
  }
</script>

<style scoped>
  .reserveContainer {
    max-width: 600px;
    margin: 0 auto;
    padding: 10px;
    font-family: Arial, sans-serif;
  }

  h2 {
    text-align: center;
    color: #333;
    font-size: 20px;
    margin-bottom: 15px;
  }
</style>



