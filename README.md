# CODE WITH TEAM

## Cel platformy

**CODE WITH TEAM** to edukacyjna inicjatywa skierowana do studentów oraz osób zainteresowanych branżą IT, którzy chcą rozwijać swoje kompetencje w zakresie:

- pracy zespołowej,
- umiejętności miękkich,
- korzystania z narzędzi takich jak Git,
- oraz praktycznego tworzenia oprogramowania.

Platforma bazuje na idei rozgrywki zespołowej — uczestnicy dobierani są do drużyn według wybranych ról (np. frontend, backend, project manager), otrzymują wspólne zadanie projektowe i muszą razem doprowadzić je do końca, zaliczając kolejne etapy projektu.

## Dla kogo?

Platforma jest przeznaczona dla:

- studentów kierunków technicznych,
- osób chcących zdobyć pierwsze doświadczenie w pracy zespołowej w IT,
- samouków i uczestników bootcampów, którzy znają podstawy programowania i chcą rozwijać się dalej w praktyce.

## Efekt końcowy

Uczestnicy kursu zbudują swoje doświadczenie poprzez udział w kilkuetapowych projektach, które zakończą się:

- stworzeniem kompletnej/ych aplikacji zespołowej/ych (gra, aplikacja desktopowa lub webowa),
- uzyskaniem certyfikatu ukończenia kursu (po zrealizowaniu kilku projektów),
- zdobyciem praktycznych umiejętności cenionych na rynku pracy, takich jak komunikacja, praca z repozytorium, dzielenie ról w projekcie czy prowadzenie demo.

## Workflow platformy

Każdy uczestnik wybiera swoją **rolę w zespole** (np. frontend, backend, tester, dokumentalista, project manager), po czym system automatycznie tworzy zespół z innych uczestników.

Pojedyńczy projekt składa się z:

- projektu z dynamicznie generowanymi wariantami (np. cyber),
- zróżnicowanych typów projektów (strony, aplikacje, gry),
- etapów (checkpoints), które drużyna musi zaliczyć,
- zadań końcowych, które wymagają połączenia wcześniej zdobytych umiejętności.

Projekt zawiera:

- automatycznie uruchamiane testy jednostkowe i integracyjne,
- wytyczne technologiczne i funkcjonalne,
- checklisty do samosprawdzenia oraz opcjonalny system peer-review.

## MVP – Minimum Viable Product

### Kluczowe funkcjonalności

1. **System logowania/rejestracji użytkowników**
   - Możliwość stworzenia konta
   - Możliwość logowania
   - Odzyskiwanie hasła

2. **Mechanizm dobierania zespołu**
   - Wybranie pożądanej roli
   - System doboru graczy zważając na ich "rozegrane" projekty oraz wybrane role
   - 

3. **Generator zadań zespołowych**
   - Dynamiczne zadania oparte na szablonach z losowaniem motywu, technologii i ograniczeń.

4. **Repozytoria zespołowe**
   - Automatyczne tworzenie repozytoriów GitHub z wstępnym kodem i testami (z użyciem GitHub API).

5. **Panel użytkownika**
   - Dostęp do bieżącego projektu, instrukcji, statusu zespołu i checklisty.

6. **Automatyczne testowanie**
   - Integracja z CI/CD (np. GitHub Actions) – uruchamianie testów po pushu.

### Stack technologiczny (propozycja MVP)

- **Frontend**: React + Tailwind CSS
- **Backend**: .NET 6/7 (ASP.NET Core)
- **Baza danych**: PostgreSQL / Azure SQL Database
- **Autentykacja**: ASP.NET Identity / OAuth 2.0 / Firebase Auth
- **CI/CD & Repo**: GitHub Actions + GitHub API
- **Hosting**: Azure (dla backendu), Vercel (dla frontu) lub Render

### Możliwości rozwoju po MVP

- Wprowadzenie systemu oceniania z peer review.
- Rozszerzenie o leaderboard i odznaki.
- System sezonów i nowych zadań co kilka miesięcy.
- Integracja z Discordem (powiadomienia zespołowe).
- Możliwość mentorstwa i feedbacku od ekspertów.
