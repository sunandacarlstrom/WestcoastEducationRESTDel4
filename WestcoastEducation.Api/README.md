# Utan databasåtkomst | WestcoastEducationRESTDel1

## Fortsättning på ett fiktivt projekt under namnet Westcoast Education (WE).

### Del 1: Skapa ett REST baserat API

I denna första del ska ingen kod skrivas för att implementera nedanstående. Endast endpoints ska skapas med korrekta http-verb och argument. Varje fråga testas i ThunderClient/Postman eftersom det är ytterst viktigt att veta att man kommer åt varje metod. 

API'et ska kunna hantera kurser, lärare samt studenter

API'et ska ha följande endpoints:

#### Kurser

Lista kurser
- Hämta kurs på kursid
- Hämta kurs på kursnummer
- Hämta kurs på kurstitel
- Hämta kurs på startdatum
- Lägga till en ny kurs
- Uppdatera en ny kurs
- Markera en kurs som fullbokad
- Markera en kurs om klar

#### Lärare

- Lista lärare
- Hämta lärare på id
- Hämta lärare på e-postadress
- Lägga till en ny lärare
- Uppdatera en lärare
- Lista vilka kurser som en lärare undervisar
- Lägga till kurs som lärare kan undervisa i

#### Studenter

- Lista studenter
- Hämta student på id
- Hämta student på personnummer
- Hämta student på e-postadress
- Lägga till ny student
- Uppdatera en student
- Lista kurser som en student är anmäld på
- Anmäla en student till nya kurser