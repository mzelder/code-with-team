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

Uczestnicy platformy zbudują swoje doświadczenie poprzez udział w kilkuetapowych projektach, które zakończą się:

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
   - Zarzadzanie profilem - edycja danych użytkownika, zmiana hasła

 2. **Mechanizm dobierania zespołu** <br>
Automatyczny matchmaking uczestników do zespołów 3–4 osobowych.

- Po rejestracji/logowaniu użytkownik **wybiera preferowaną rolę** (np. frontend, backend, tester).
- System analizuje dostępnych użytkowników, ich **historię projektów** i **wybrane role**, by stworzyć **zbalansowany zespół**:
  - unika powtarzających się zespołów,
  - paruje osoby z różnym doświadczeniem (jeśli to możliwe),
  - stara się dopasować kompletne zespoły pod konkretne zadania.
- Przypisanie użytkownika do zespołu i **stworzenie “pokoju projektowego”**, widocznego w panelu.

3. **Generator zadań zespołowych** <br>
Zadania generowane automatycznie na podstawie szablonów. <br>
Stworzenie przynajmniej 2 szablonów. <br>

- System losuje:
  - **typ projektu** (np. gra 2D, aplikacja webowa, narzędzie CLI),
  - **motyw** (np. “hakerzy”, “ekologia”, “nauka przez zabawę”),
  - **ograniczenia techniczne** (np. “brak frameworka”, “dark mode only”, “limit API calls”).
- Dla każdego projektu tworzony jest **pakiet startowy**:
  - krótki opis zadania,
  - role i odpowiedzialności,
  - checklisty do realizacji.

4. **Repozytoria zespołowe** <br>
Automatyczne tworzenie repozytoriów GitHub z wstępnym kodem i testami (z użyciem GitHub API).
- Po stworzeniu zespołu i przypisaniu zadania, system:
  - tworzy **nowe repozytorium zespołu** (prywatne/publiczne),
  - dodaje członków zespołu jako kolaboratorów,
  - wrzuca **starter-kit** (README, struktura projektu, testy, CI/CD).
- Repozytorium zawiera linki do: instrukcji, checklisty i statusu zadania.

5. **Automatyczne testowanie (CI/CD)** <br>
Sprawdzenie projektu bez udziału mentora.<br>

- Każde zadanie będzie miało przypisane **testy jednostkowe i integracyjne**, które uruchamiają się:
  - przy każdym `pushu` do repozytorium,
  - lub na żądanie (np. z poziomu platformy).
- Wdrożenie przy pomocy **GitHub Actions**:
  - budowanie projektu,
  - uruchamianie testów,

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
