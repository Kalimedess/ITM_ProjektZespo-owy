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
import editItems from '@/views/admin/editItems.vue'
import decisionHistoryView from '@/views/game/gameDecisionHistoryView.vue'
import testBoard from '@/views/testBoard.vue'
import editBitsView from '@/views/game/editBitsView.vue'
import decisionPanel from '@/views/game/decisionPanelView.vue'
import blockCards from '@/views/game/blockCardsView.vue'
import resetPasswordView from '@/views/resetPasswordView.vue'
import confirmEmailView from '@/views/confirmEmailView.vue'
import apiServices from '@/services/apiServices'
import apiConfig from '@/services/apiConfig'
import gameView from '@/views/admin/adminGameView.vue'

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
      path:'/confirm/:token',
      name:'confirm-email',
      component: confirmEmailView,
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
      name: 'admin-game-dashboard',
      component: adminGameDashboardView,
      meta: { requiresAuth: true },
      children: [
        {
          path: ':gameId',
          name: 'table-view',
          component: gameView,
          props: true,
          meta: { requiresAuth: true }
        },
        {
          path: '/admin/game/:gameId/decision',
          name: 'decision-panel',
          component: decisionPanel,
          props: true,
          meta: { requiresAuth: true }
        },
        {
          path: '/admin/game/:gameId/:teamId',
          name: 'decisionanel',
          component: decisionPanel,
          props: true,
          meta: { requiresAuth: true }
        },
        {
          path: '/admin/game/:gameId/statistics',
          name: 'admin-game-statistics',
          component: gameStatistics,
          props: true,
          meta: { requiresAuth: true }
        },
        {
          path: '/admin/game/:gameId/editbits',
          name: 'edit-bits',
          component: editBitsView,
          props: true,
          meta: { requiresAuth: true }
        },
        {
          path: '/admin/game/:gameId/blockcards',
          name: 'block-cards',
          component: blockCards,
          props: true,
          meta: { requiresAuth: true }
        },

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
    await apiServices.get(apiConfig.auth.me);
    next();
  } catch (err) {
    sessionStorage.setItem('showLoginAfterRedirect', 'true');
    next('/');
    console.log('Error: ',err)
  }
});



export default router