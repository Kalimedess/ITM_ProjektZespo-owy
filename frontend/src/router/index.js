import { createRouter, createWebHistory } from 'vue-router'
import mainView from '@/views/landing/mainView.vue'
import adminDashboardView from '@/views/admin/adminDashboardView.vue'
import homeAdmin from '@/views/admin/homeAdminView.vue'
import statisticsView from '@/views/admin/adminStatistics.vue'
import editBoardView from '@/views/admin/editBoardView.vue'
import cheatSheetView from '@/views/admin/cheatSheetView.vue'
import editCardsView from '@/views/admin/editCardsView.vue'
import adminGameDashboardView from '@/views/game/adminGameDashboardView.vue'
import playerdView from '@/views/player/playerView.vue'
import gameStatistics from '@/views/game/gameStatistics.vue'

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
    },
    {
      path: '/admin/game',
      component: adminGameDashboardView,
      children: [
        {
          path: '',
          name: 'admin-game-statistics',
          component: gameStatistics 
        }
      ]
    },
    {
      path:'/player',
      name:'player-dashboard',
      component: playerdView,
    },
  ]
})

export default router