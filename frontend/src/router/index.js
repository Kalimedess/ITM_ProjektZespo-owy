import { createRouter, createWebHistory } from 'vue-router'
import axios from 'axios'
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
import editItems from '@/views/admin/editItems.vue'
import decisionHistoryView from '@/views/game/gameDecisionHistoryView.vue'
import testBoard from '@/views/testBoard.vue'
import editBitsView from '@/views/game/editBitsView.vue'
import resetPasswordView from '@/views/resetPasswordView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path:'/',
      name:'main',
      component:mainView,
    },
    {
      path:'/testBoard',
      name:'test-board',
      component:testBoard,
    },
    {
      path:'/resetPassword/:token',
      name:'reset-password',
      component:resetPasswordView,
    },
    { 
      path: '/admin',
      name: 'admin-dashboard',
      component: adminDashboardView,
      meta: { requiresAuth: true },
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
          component: editBoardView,
        },
        {
          path:'cheatSheet',
          name:'cheat-sheet',
          component: cheatSheetView,
        },
        {
          path:'editCards',
          name:'edit-cards',
          component: editCardsView,
        },
        {
          path:'editItems',
          name:'edit-items',
          component: editItems,
        }
      ]
    },
    {
      path: '/admin/game',
      component: adminGameDashboardView,
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          name: 'admin-game-statistics',
          component: gameStatistics,
        },
        {
          path: 'editbits',
          name: 'edit-bits',
          component: editBitsView,
        }

      ]
    },
    {
      path:'/player',
      name:'player-dashboard-by-token',
      component: playerdView,
    },
    {
      path:'/player/:teamToken',
      name:'player-dashboard',
      component: playerdView,
      props:true
    },
    {
      path:'/tempdecisions',
      name:'decision-history',
      component: decisionHistoryView,
    },
  ]
})

//Zabazpiecznie routera 
router.beforeEach(async (to, from, next) => {
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);

  if (!requiresAuth) return next();

  try {
    await axios.get('http://localhost:5023/api/auth/me', {
      withCredentials: true
    });
    next();
  } catch (err) {
    sessionStorage.setItem('showLoginAfterRedirect', 'true');
    next('/');
  }
});



export default router
