## Konfiguracja MySQL

### Instalacja MySQL Server

    [Pobierz](https://dev.mysql.com/downloads/installer/) i zainstaluj MySQL Installer Community

    Zainstaluj tylko MySQL Server 
    PAMIĘTAJ! Podczas konfigurtacji serwera Hasło roota ustaw na 'root'


## Konfiguracja DOTNET

### Instalacja .NET

    [Pobierz](https://dotnet.microsoft.com/en-us/download) i zanistaluj .NET 8.0 

    Następnie zainstaluj Microsoft.EntityFrameworkCore używając:
    `dotnet tools install -global dotnet-ef `

    Następnie sprawdź czy dotnet-ef został zainstalowany:
    `dotnet tool list -g `

### Tworzenie bazy danych

    Wchodząc na serwer MySQL utwór nową Bazę Danych o nazwie 'DigitalWars'

    Następnie stwórz wszystkie tabele poleceniem:
    `dotnet ef database update`
    Jeżeli nie zadziała stwórz migrację i spróbuj ponownie
    `dotnet ef migrations add DatabaseCreatedByNazwaGit `

    Wszysko powinno teraz działać




