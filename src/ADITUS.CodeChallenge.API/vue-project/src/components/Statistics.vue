<template>
  <div>
    <h1>Statistiken</h1>
    <div class="statistics">
      <canvas ref="chartCanvas"></canvas>
    </div>
  </div>
</template>


<script>
import { ref, onMounted } from 'vue';
import { Chart, BarController, BarElement, CategoryScale, LinearScale, Title, Tooltip, Legend } from 'chart.js';

Chart.register(BarController, BarElement, CategoryScale, LinearScale, Title, Tooltip, Legend);

  export default {
    name: 'EventChart',
    setup() {
      const chartCanvas = ref(null);
      const chartInstance = ref(null);
   
      /* Funktion zum aufrufen der API Daten */
      const fetchEventData = async () => {
        try {
          const response = await fetch('events'); // Daten von Controller aufrufen
          const data = await response.json(); // Daten in Json umwandeln

          const organizedData = organizeDataByEventType(data);
          
          createChart(organizedData);
        } catch (error) {
          console.error('Error fetching event data:', error);
        }
      };

      const organizeDataByEventType = (data) => {
        // Dataset für jedes Event
        const datasets = [];

        data.events.forEach(event => {
          const titleEvent = event.name + " (" + event.type + ")";
          const stats = data.statisticsEvents.find(stat => stat.id === event.id);
          const dataset = {
            label: titleEvent, 
            data: [0, 0, 0, 0, 0, 0, 0],
            backgroundColor: '',
            borderColor: '',
            borderWidth: 1,
            borderRadius: 10,
          };
          
          // Statistiken Daten je nach Eventtyp trennen und ausfüllen
          if (stats) {
            if (event.type == "OnSite" || event.type == "Hybrid") {
              if (stats.onSite) {
                dataset.data[0] = stats.onSite.visitorsCount;
                dataset.data[1] = stats.onSite.exhibitorsCount;
                dataset.data[2] = stats.onSite.boothsCount;
                dataset.backgroundColor = 'rgba(255, 0, 15, 0.3)';
                dataset.borderColor = 'rgba(255, 0, 15, 1)';
              }
            }

            if (event.type == "Online" || event.type == "Hybrid") {
              if (stats.online) {
                dataset.data[3] = stats.online.attendees;
                dataset.data[4] = stats.online.invites;
                dataset.data[5] = stats.online.visits;
                dataset.data[6] = stats.online.virtualRooms;
                dataset.backgroundColor = 'rgba(54, 162, 235, 0.5)';
                dataset.borderColor = 'rgba(54, 162, 235, 1)';
              } 
            }

            if (event.type == "Hybrid") {
              dataset.backgroundColor = 'rgba(255, 235, 129, 0.5)';
              dataset.borderColor = 'rgba(255, 235, 129, 1)';
            } 
          }
          // Daten der Statistiken in datasets speichern
          datasets.push(dataset); 
        });

        return {
          datasets
        };
      };

      /* Funktion zum Erstellung der Statistiken */
      const createChart = (organizedData) => {
        const { datasets } = organizedData;

        // Chart erstellen
        chartInstance.value = new Chart(chartCanvas.value, {
          type: 'bar',
          data: {
            labels: ['Besucher Anzahl', 'Austeller Anzahl', 'Stände Anzahl', 'Teilnehmerinnen', 'Eingeladen', 'Besuche', 'Virtuelle Räume'], // Etiketten für die Daten
            datasets: datasets,
          },
          options: {
            responsive: true,
            plugins: {
              legend: {
                position: 'top',
              },
              title: {
                display: true,
                text: 'Event Statistics',
              },
            },
            scales: {
              y: {
                beginAtZero: true,
                title: {
                  display: true,
                },
              },
            },
          },
        });
      };

      onMounted(fetchEventData);

      return {
        chartCanvas,
      };
    },
  };
</script>

<style scoped>

  .statistics{
      width:950px;
      height:750px;
  }
  canvas {
    max-width: 100%;
    height: 500px;
    background: white;
  }
</style>

