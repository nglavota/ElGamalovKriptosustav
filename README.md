# ElGamalov kriptosustav

Kroz ovaj projekt je implementiran ElGamalov kriptosustav u programskom jeziku C#. 

## Sadržaj 
- O projektu
- Funkcionalnosti
- Upute za korištenje
- Primjer rada

## O projektu

ElGamalov sustav šifriranja je asimetrični algoritam koji se oslanja na kriptografiju s javnim ključem, a temelji se na razmjeni ključeva prema Diffie-Hellmanovoj metodi. Oslanja se na izazove u izračunu diskretnih logaritama unutar
konačnih polja i koristi za osiguravanje sigurne komunikacije putem nesigurnih kanala.

ElGamalov algoritam se izvodi u tri faze:
- generiranje ključa
- šifriranje
- dešifriranje

Proces generiranja ključa uključuje odabir velikog prostog broja p, primitivnog korijena α od p i slučajnog cijelog broja a tako da je 1 ≤ a ≤ p − 1. Prostor otvorenih tekstova tada je P = Z<sub>p</sub>*, prostor šifrata
C = Z<sub>p</sub>* × Z<sub>p</sub>* i prostor ključeva K = {(p, α, a, β) : β = α<sup>a</sup> mod p}. Vrijednosti p, α i β su javne, a vrijednost a je tajna.

## Funkcionalnosti

- Provjera je li broj prost (`JeProst`).  
- Pronalaženje svih primitivnih korijena za zadani \(p\).  
- Šifriranje poruke prema ElGamalovoj metodi (`Šifriraj`).  
- Dešifriranje poruke pomoću tajnog ključa (`Dešifriraj`).  
- Validacija korisničkog unosa i rukovanje pogreškama.  

## Upute za korištenje

1. Pokrenite program (u Visual Studio 2022 ili bilo kojem IDE-u koji podržava .NET 6 ili noviji).
2. Odaberite želite li **šifrirati (`s`)** ili **dešifrirati (`d`)** poruku.  
3. Unesite **prosti broj \(p\)**. Program pronalazi primitivne korijene.  
4. Odaberite **primitivan korijen** za operaciju prema rednom broju pod kojim se nalazi.  
5. Ako šifrirate:
   - Unesite **tajni ključ \(a\)** (1 < a < p-1).  
   - Program izračunava **javni ključ beta**.  
   - Unesite numeričku poruku manju od \(p\).  
   - Unesite jednokratni ključ \(k\).  
   - Program vraća šifriranu poruku `(y1, y2)`.  
6. Ako dešifrirate:
   - Unesite **tajni ključ \(a\)**.  
   - Unesite šifriranu poruku `(y1, y2)`.  
   - Program vraća originalnu poruku.  

## Primjer rada

**Šifriranje:**

Želite li šifrirati ili dešifrirati poruku? s  
Unesite prost broj p: 31  
Primitivni korijeni modulo p:  
1: 3  
2: 11  
3: 12  
4: 13  
5: 17  
6: 21  
7: 22  
8: 24  
Odaberite primitivan korijen (unesite broj koji odgovara korijenu): 1  
Unesite svoj tajni ključ a (1 < a < p-1): 8  
Primitivni korijen alfa: 3  
Javni ključ beta: 20  
Unesite poruku: 19  
Unesite jednokratni ključ k: 5  
y1 (alfa^k mod p): 26  
y2 (x * beta^k mod p): 10  
Šifrirana poruka: (y1: 26, y2: 10)  

**Dešifriranje:**

Želite li šifrirati ili dešifrirati poruku? d   
Unesite prost broj p: 31    
Primitivni korijeni modulo p:  
1: 3  
2: 11  
3: 12  
4: 13  
5: 17  
6: 21  
7: 22  
8: 24  
Odaberite primitivan korijen: 1  
Unesite svoj tajni ključ a (1 < a < p-1): 8  
Primitivni korijen alfa: 3  
Javni ključ beta: 20  
Unesite šifriranu poruku (y1 y2) (y1 i y2 odvojeni razmakom): 26 10  
Dešifrirana poruka: 19
