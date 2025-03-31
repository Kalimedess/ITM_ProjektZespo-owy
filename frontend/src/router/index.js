import { createRouter, createWebHistory } from 'vue-router'
import mainView from '@/views/mainView.vue'
import adminDashboardView from '@/views/adminDashboardView.vue'
import homeAdmin from '@/views/homeAdminView.vue'
import statisticsView from '@/views/adminStatistics.vue'
import editBoardView from '@/views/editBoardView.vue'
import cheatSheetView from '@/views/cheatSheetView.vue'
import editCardsView from '@/views/editCardsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path:'/',
      name:'main',
      component:mainView,
    },
    { 
      path: '/admin',
      name: 'admin-dashboard',
      component: adminDashboardView,
      children: [
        {
          path:'',
          name:'admin-home',
          component:homeAdmin,
        },
        {
          path:'statistics',
          name:'admin-statistics',
          component: statisticsView,
        },
        {
          path:'editBoard',
          name:'edit-board',
          component: editBoardView
        },
        {
          path:'cheatSheet',
          name:'cheat-sheet',
          component: cheatSheetView
        },
        {
          path:'editCards',
          name:'edit-cards',
          component: editCardsView
        }
      ]
    }
  ]
})

export default router