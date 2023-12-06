# Társasjáték keretrendszer ASP.NET Core és React tecnológiákkal

Ez a program, amely a Board Game Framework névre hallgat, egy webalkalmazás, amely alkalmas többféle társasjáték online, valós idejű játszásásra. Emellett támogatást nyújt fejlesztőknek, hogy egy keretrendster segítségével saját játékokat valósíthassanak meg. Jelenleg egy sakk és egy amőba játék van implementálva.

## A program felépítése

A ReactChess mappa tartalmazza a webalkalmazás frontend és backend részét egyaránt, az adatbázis-kezeléssel együtt. Ennek a projektnek az indításával indul el az alkalmazás. 

A FramewrokBackend mappa tartalmazza a keretrendszer absztrakt ősosztályait, amelyből a közreműködő fejlesztőknek is ki kell indulni.

A ConsoleChess a sakk üzleti logikáját tartalmazza, amely API-ként is működik bármilyen megjelenítési réteg számára. A TicTacToe ehhez hasonló, csak az amőbával kapcsolatban.

A ChessTest tartalmazza a sakk üzleti logikáját ellenőrző egységteszteket.

## A futtatáshoz szükséges programok

- Microsoft Visual Studio
- .NET 6 SDK
- Visual Studio Code
- Microsoft SQL Server
- Node.js


## A program futtatása

A program javasolt használata: Mivel az alkalmazás ki lett telepítve Azure segítségével felhőbe, a legegyszerűbb az alábbi linken keresztül elérni: https://reactboardgame.azurewebsites.net/ 
A program használatához szükséges két felhasználót is regisztrálni, akik ezután egymással játszhatják az egyes játékokat.

Alternatívaként az alábbi lépésekkel lokálisan is futtatható:

1. Nyisd meg a ReactChess/ClientApp mappát például VS Code segítségével!
2. Add ki az `npm install` parancsot! Lehetséges, hogy egy régen frissített modul miatt függőségi konfliktusok lesznek, ezt `npm install --legacy-peer-depths` segtségével lehet áthidalni.
3. Nyisd meg a ConsoleChess.sln-t a Visual Studio-ban!
4. A Package Manager Console-ban add ki az `Update-Database` parancsot!
5. A létrejött adatbázis connection stringjét írd be a ReactChess projekt appsettings.json fájlban.
6. Indítsd el a ReactChess projektet!

Ugyanúgy, mint az Azure-ra telepített program esetében, legalább két felhasználót kell regisztrálni az alkalmazás kipróbálásához.