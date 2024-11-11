import { createRouter, createWebHistory } from 'vue-router'
import ShowReservations from '../views/ShowReservationsView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/statistics',
      name: 'statistics',
      component: () => import('../views/StatisticsView.vue') ,
    },
    {
      path: '/reservation',
      name: 'Reservation',
      component: () => import('../views/ReservationView.vue'),
    },
    {
      path: '/allReservation',
      name: 'ShowReservation',
      component: ShowReservations,
    },
  ],
})

export default router
