import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ShowReservations from '../views/ShowReservationsView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
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
