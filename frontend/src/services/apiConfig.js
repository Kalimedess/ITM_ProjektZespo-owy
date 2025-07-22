const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5023/api';

const endpoints = {
  auth: {
    login: '/auth/login',
    register: '/auth/register',
    logout: '/auth/logout',
    me: '/auth/me', // Get current user
    confirmEmail: (token) =>`/auth/confirm/${token}`,
    forgotPassword: '/password/reset-password',
    resetPassword: '/password/reset',
    validateResetToken: (token) => `/password/validate-token/${token}`,
  },
  admin: {
    deck: {
        getAll: '/admin/deck/get',
        upload: '/admin/deck/upload',
        decisions: '/admin/deck/decisions'
    },
    settings: {
        licenses: '/admin/licenses'
    }
  },
  games: {
    create: '/games/create',
    getById: (id) => `/games/${id}`,
    getAll: '/games/active',
    updateStatus: (id) => `/games/${id}/status`,
    stopAll: '/games/stop-all',
    endAll: '/games/end-all',
    getTeamsManagement: (gameId) => `/player/game/${gameId}/teams-management`,
    updateTeamBudget: (teamId) => `/player/team/${teamId}/budget`,
    getDecisionCards: (gameId) => `/player/game/${gameId}/decision-cards`,
    unlockCard: (gameId) => `/player/game/${gameId}/unlock-card`
  },
  boards: {
    create: '/board/add',
    getAll: '/board/get',
    delete: (id) => `/board/delete/${id}`,
    update: (id) => `/board/edit/${id}`,
  },
  player: {
    getTeamInfo: (teamToken) => `/player/team/${teamToken}`,
    playCardSuccess: (cardId) => `/player/success/${cardId}`,
    playCardFailure: (cardId) => `/player/failure/${cardId}`,
    getCards: (deckId) =>`player/deck/${deckId}/unified-cards`,
    getLogs: '/player/getLogs',
    getCurrency: '/player/getCurrency'
  },
  processes: {
    getByDeck: (deckId) => `/processes/by-deck/${deckId}`
  }
  
};

export default {
  baseURL: API_BASE_URL,
  ...endpoints
};