<template>
  <h2>Aktuelle Reservierungen</h2>
  <div class="reserveContainer">

    <ul>
      <li v-for="reservation in allReservations" :key="reservation.id" class="reservation-item">
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
          const response = await fetch('/reservation/allReservations'); // Zugriff auf Controller /reservation
          
          allReservations.value = await response.json();
          console.log(allReservations);
        } catch (error) {
          console.error('Error fetching hardware data:', error);
        }
      };

      onMounted(fetchAllReservations);

      return {
        allReservations,

      };
    },
  }
</script>

<style scoped>
  .reservationTable {
    width: 100%;
    border-collapse: collapse;
    margin-top: 20px;
  }

    .reservationTable th,
    .reservationTable td {
      border: 1px solid #ddd;
      padding: 8px;
      text-align: left;
    }

    .reservationTable th {
      background-color: #ada4a3;
      font-weight: bold;
    }
</style>



