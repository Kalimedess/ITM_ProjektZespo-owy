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
    },
    games: {
      getGames: (gameId) => `adminpanel/games/${gameId}`,
      getTeams: (gameId) => `adminpanel/teams/by-game/${gameId}`
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
    getItemCards: (gameId) => `/player/game/${gameId}/item-cards`, 
    unlockCard: (gameId) => `/player/game/${gameId}/unlock-card`,
    getPendingLogs: (gameId) => `/player/game/${gameId}/pending-logs`, 
    approveLog: (logId) => `/player/approve-log/${logId}`,           
    rejectLog: (logId) => `/player/reject-log/${logId}`,
    getGameEvents: '/player/game-events',
    applyEvent: (gameId) => `/player/game/${gameId}/apply-event`,   
    getHistoryVersion: (gameId) => `/player/game/${gameId}/history-version`,
    getPendingVersion: (gameId) => `/player/game/${gameId}/pending-version`,
    getGameData: `/player/gameRivalBoard`,
    getEnablersMap: `/admin/enablersMap`,
    getTeamEntry: `admin/latestEntries`         
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
    getCurrency: '/player/getCurrency',
    getPawns: '/player/team-board',
    getRivalPawns: `/player/rival-board`,
    getPlayerHistory: '/player/player-history',
    getPlayerHistoryVersion: (gameId, teamId) => `/player/game/${gameId}/history-version?teamId=${teamId}`

  },
  processes: {
    getByDeck: (deckId) => `/processes/by-deck/${deckId}`
  }
  
};

export default {
  baseURL: API_BASE_URL,
  ...endpoints
};