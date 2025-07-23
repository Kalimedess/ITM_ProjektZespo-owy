// W pliku src/services/signalrService.js

import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5023/gameHub")
    .withAutomaticReconnect()
    .build();

// Zamiast flagi, będziemy przechowywać "obietnicę" (Promise) połączenia
let startPromise = null;

export default {
    connection,
    
    // Ostateczna, poprawna funkcja startująca
    start() {
        // Jeśli nie rozpoczęliśmy jeszcze łączenia, stwórz nową obietnicę
        if (!startPromise) {
            console.log("SignalR: Inicjowanie nowego połączenia...");
            startPromise = connection.start().catch(err => {
                // W razie błędu, zresetuj obietnicę, aby umożliwić ponowną próbę
                console.error("SignalR: Błąd podczas startu, resetowanie obietnicy.", err);
                startPromise = null;
                throw err; // Rzuć błąd dalej
            });
        }
        
        // Zawsze zwracaj istniejącą lub nowo utworzoną obietnicę
        return startPromise;
    },
    
    joinGameRoom: (gameId) => {
        if (connection.state === signalR.HubConnectionState.Connected) {
            return connection.invoke("JoinGameRoom", gameId.toString());
        }
    },
    
    leaveGameRoom: (gameId) => {
        if (connection.state === signalR.HubConnectionState.Connected) {
            return connection.invoke("LeaveGameRoom", gameId.toString());
        }
    }
};