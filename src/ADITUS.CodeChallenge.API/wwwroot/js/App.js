import { createBarChart } from '../vue/BarChart.js';

//Esto funciona: ->
const { createApp, ref, onMounted, createChart } = Vue
createApp({
  data() {
    return {
      events: [], // Array with Information about Events
      statisticsEvents: [], // Array to store the combined event data
      eventName: '',
      dataFromStatistic: null,
      chartData: {
        labels: [], values: []
      }
    };
  },
      // Usa onMounted para hacer la solicitud a la API al cargar el componente
     mounted() {
     this.fetchEvents();
    },
    methods: {
    async fetchEvents() {
        try {
          const response = await fetch('events'); // Cambia esta URL según sea necesario
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
          const data = await response.json(); // recibo response en json
          this.events = data.events; // Asigno los datos recibidos a 'events'
          this.statisticsEvents = data.statisticsEvents; // Asigna los datos recibidos a 'statisticsEvents'
          this.$nextTick(() => {
            this.events.forEach(event => {
              const statistics = this.statisticsEvents.find(stat => stat.id === event.id);
              if (statistics) {
                const ctx = document.getElementById(`chart-${event.id}`).getContext('2d');
                const chartData = this.getChartData(statistics);
                console.log(`Contexto Canvas para ${event.id}:`, ctx);
                console.log("Labels:", chartData.labels);
                console.log("Values:", chartData.values);
                createBarChart(ctx, chartData.labels, chartData.values);
              }
            });
          });
        } catch (error) {
          console.error('Error al obtener eventos:', error);
        }

      },
        //recorremos el mapa de eventos para sacar el atributo eventNames
        //this.eventNames = this.events.map(event => event.name);
        //this.dataFromStatistic = this.statisticsEvents.map(data => statisticsEvents.visitorsCount);
        //console.log(this.dataFromStatistic);
        //const ctx = "";
        //data.events.forEach(
        //  // mi contexto para crear mi chart
        //  ctx = document.getElementsByClassName('myBarChart').getContext('2d');
        //// aca llamo la funcion createBarChart que importe del otro Datei
        //createBarChart(ctx, this.eventNames, this.statisticsEvents[0]);
          getChartData(statistics) {
            // Configura los datos del gráfico para cada tipo de evento
            if (statistics.onSite || statistics.hybrid) {
              return {
                labels: ['Besucher', 'Aussteller', 'Stände'],
                values: [
                  statistics.onSite.visitorsCount,
                  statistics.onSite.exhibitorsCount,
                  statistics.onSite.boothsCount
                ]
              };
            } else if (statistics.online || statistics.hybrid) {
              return {
                labels: ['Teilnehmer', 'Eingeladen', 'Besuche', 'Virtuelle Räume'],
                values: [
                  statistics.online.attendees,
                  statistics.online.invites,
                  statistics.online.visits,
                  statistics.online.virtualRooms
                ]
              };
            }
            return { labels: [], values: [] };
          }
    },
  template: `
    <div class="myChart">
      <h1>Statistiken</h1>
       <div v-for="(event, index) in events" :key="event.id">
          <h2>{{ event.name }}</h2>
          <canvas :id="'chart-' + event.id"></canvas>
        </div>
    </div>
  `
}).mount('#app');

//-------esto si funciona y me muestra estadisticas
//createApp({
//  data() {
//    return {
//      events: [], // Array with Information about Events
//      statisticsEvents: [], // Array to store the combined event data
//      eventName: '',
//      dataFromStatistic: ''
//    };
//  },
//  mounted() {
//    this.fetchEvents();
//  },
//  methods: {
//    async fetchEvents() {
//      try {
//        const response = await fetch('events');
//        const data = await response.json();
//        this.events = data.events;
//        this.statisticsEvents = data.statisticsEvents;

//        // Extraer nombres y contar visitantes
//        this.eventNames = this.events.map(event => event.name);
//        this.visitorCounts = this.statisticsEvents.map(event => {
//          const stat = this.statisticsEvents.find(stat => stat.id === event.id);
//          return stat ? stat.onSite.visitors : 0;
//        });

//        // Llamar a la función que dibuja el gráfico
//        this.createChart();
//      } catch (error) {
//        console.error('Error al obtener eventos:', error);
//      }
//    },
//    createChart() {
//      const ctx = document.getElementById('myBarChart').getContext('2d');
//      new Chart(ctx, {
//        type: 'bar',
//        data: {
//          labels: this.eventNames,
//          datasets: [{
//            label: 'Número de Visitantes',
//            data: this.statistik,
//            backgroundColor: 'rgba(75, 192, 192, 0.2)',
//            borderColor: 'rgba(75, 192, 192, 1)',
//            borderWidth: 1
//          }]
//        },
//        options: {
//          scales: {
//            y: {
//              beginAtZero: true
//            }
//          }
//        }
//      });
//    }
//  },
//  template: `
//    <div>
//      <h1>Veranstaltungsstatistiken</h1>
//      <canvas id="myBarChart"></canvas>
//    </div>
//  `
//}).mount('#app');
