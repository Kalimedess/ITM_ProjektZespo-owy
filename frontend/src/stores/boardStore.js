import { defineStore } from 'pinia'
import axios from 'axios'

const api = axios.create({
    baseURL: 'http://localhost:3001',
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
    }
});

export const useBoardStore = defineStore('board', {

    state: () => ({
        boards: [],
        currentBoardId: null,
        isLoading: false,
        error: null,
    }),

    getters: {

        currentBoard: (state) => {
            if (!state.currentBoardId) return null;
            return state.boards.find(board => board.id === state.currentBoardId);
        },

        getBoardById: (state) => (id) => {
            return state.boards.find(board => board.id === id);
        }
    },

    actions: {

        async fetchBoards() {
            try {
                this.isLoading = true;
                this.error = null;

                const response = await api.get('/boards');
                this.boards = response.data;

                if (this.boards.length > 0 && !this.currentBoardId) {
                    this.currentBoardId = this.boards[0].id
                }

                return this.boards;
            } catch (error) {
                this.error = error.response?.data?.message || error.message;
                console.error('Błąd podczas pobierania plansz:', error);
                throw error;
            } finally {
                this.isLoading = false;
            }
        },

        //Funkcja do stworzenia nowej planszy
        async createBoard(boardData = {}) {
            try {
                this.isLoading = true;
                this.error = null;

                //Walidacja wszystkich pól

                //Walidacja nazwy
                if (!boardData.name) {
                    throw new Error("Nazwa planszy jest wymagana");
                }

                //Weryfikacja etykiet
                if (!boardData.config ||
                    !boardData.config.labelsUp ||
                    !boardData.config.labelsRight ||
                    !boardData.config.descriptionDown ||
                    !boardData.config.descriptionLeft) {
                    throw new Error("Wszystkie etykiety planszy są wymagane");
                }

                //W mojej aktualnej implementacji planszy na każdy label przypadają 2 komórki, 
                //więc ilość rzędów i kolumn jest od nich zależna.
                const cols = boardData.config.labelsUp.length * 2;
                const rows = boardData.config.labelsRight.length * 2;

                //Domyśle wartości dla kolorów gdyby przy tworzeniu użytkownik ich nie podał.
                const defaultColors = {
                    borderColor: "#595959",
                    cellColor: "#fefae0",
                    borderColors: ["green", "yellow", "orange", "red"]
                }

                const fixedMargin = 40;

                //Przygotowanie danych planszy
                const newBoard = {
                    name: boardData.name,
                    createdAt: new Date().toISOString(),
                    updatedAt: new Date().toISOString(),
                    config: {
                        ...boardData.config,
                        rows: rows,
                        cols: cols,

                        //Nadpisanie lub ustawienie domyślych kolorów 
                        //zależnie od tego czy użytkownik podał kolory
                        borderColor: boardData.config.borderColor || defaultColors.borderColor,
                        cellColor: boardData.config.cellColor || defaultColors.cellColor,
                        borderColors: boardData.config.borderColors || defaultColors.borderColors,

                        //W mojej implementacji margines jest stały bo inne źle wyglądają
                        margin: fixedMargin
                    }
                };

                //POST na serwer z nową planszą
                const response = await api.post('/boards', newBoard);

                const savedBoard = response.data;

                //Dodanie nowej planszy do lokalnego stanu
                this.boards.push(savedBoard);
                this.currentBoardId = savedBoard.id;

                return savedBoard;
            } catch (error) {
                this.error = error.response?.data?.message || error.message;
                console.error('Błąd podczas tworzenia planszy:', error);
                throw error;
            } finally {
                this.isLoading = false;
            }
        },

        // Funkcja do aktualizowania parametrów planszy
        async updateBoard(boardId, config) {
            try {
                this.isLoading = true;
                this.error = null;

                const boardToUpdateIndex = this.boards.findIndex(board => board.id === boardId);
                if (boardToUpdateIndex === -1) {
                    throw new Error(`Nie znaleziono planszy o ID ${boardId}`);
                }

                const updatedBoard = {
                    ...this.boards[boardToUpdateIndex],
                    config: { ...this.boards[boardToUpdateIndex].config, ...config },
                    updatedAt: new Date().toISOString()
                };

                const response = await api.put(`/boards/${boardId}`, updatedBoard);

                const savedBoard = response.data;

                this.boards[boardToUpdateIndex] = savedBoard;

                return savedBoard;
            } catch (error) {
                this.error = error.response?.data?.message || error.message;
                console.error(`Błąd podczas aktualizacji planszy o ID ${boardId}:`, error);
                throw error;
            } finally {
                this.isLoading = false;
            }
        },

        // Funkcja do usunięcia planszy
        async deleteBoard(boardId) {
            try {
                this.isLoading = true;
                this.error = null;

                // Wyślij żądanie usunięcia do serwera przy użyciu Axios
                await api.delete(`/boards/${boardId}`);

                // Usuń planszę z lokalnego stanu
                const boardIndex = this.boards.findIndex(board => board.id === boardId);
                if (boardIndex !== -1) {
                    this.boards.splice(boardIndex, 1);

                    // Jeśli usunęliśmy aktualnie wybraną planszę, wybierz inną lub ustaw null
                    if (this.currentBoardId === boardId) {
                        this.currentBoardId = this.boards.length > 0 ? this.boards[0].id : null;
                    }
                }

                return { success: true, id: boardId };
            } catch (error) {
                this.error = error.response?.data?.message || error.message;
                console.error(`Błąd podczas usuwania planszy o ID ${boardId}:`, error);
                throw error;
            } finally {
                this.isLoading = false;
            }
        },

        // Ustawia aktualnie wybraną planszę
        setCurrentBoard(boardId) {
            this.currentBoardId = boardId;
        },

        // Ładuje domyślną planszę lub tworzy nową, jeśli nie ma żadnej
        async loadDefaultBoard() {
            try {
                // Sprawdź, czy mamy już jakieś plansze
                if (this.boards.length === 0) {
                    // Pobierz plansze z serwera
                    await this.fetchBoards();
                    
                    // Jeśli nadal nie ma żadnych plansz, utwórz domyślną
                    if (this.boards.length === 0) {
                        await this.createBoard({
                            name: 'Plansza domyślna',
                            config: {
                                labelsUp: ['Podstawowa kordynacja', 'Standaryzacja procesów', 'Zintegrowane działania', 'Pełna integracja strategiczna'],
                                labelsRight: ['Nowicjusz', 'Naśladowca', 'Innowator', 'Lider cyfrowy'],
                                descriptionDown: "Poziom integracji wew/zew",
                                descriptionLeft: "Zawansowanie Cyfrowe",
                            }
                        });
                    }
                }
                
                // Upewnij się, że mamy wybraną bieżącą planszę
                if (!this.currentBoardId && this.boards.length > 0) {
                    this.currentBoardId = this.boards[0].id;
                }
                
                return this.currentBoard;
            } catch (error) {
                console.error('Błąd podczas ładowania domyślnej planszy:', error);
                this.error = error.response?.data?.message || error.message;
                throw error;
            }
        }
    }
});