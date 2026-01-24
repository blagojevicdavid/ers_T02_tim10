# 🍷 Vinariја Monte Rosso – Informacioni sistem

Ovaj repozitorijum sadrži kompletno implementiran informacioni sistem za upravljanje radom **vinariје Monte Rosso** u Toskani. Projekat je realizovan u okviru predmeta **Elementi razvoja softvera (ERS)** i u potpunosti prati zadatu projektnu specifikaciju za temu **T02 – Vinariја**.

Sistem je razvijen kao **konzolna aplikacija**, strogo podeljena po slojevima, uz doslednu primenu **SOLID principa**, **Dependency Injection‑a**, repozitorijumskog obrasca i testiranja.

---

## 📌 Opšti opis sistema

Informacioni sistem pokriva kompletan tok poslovanja vinariје:

* 🌱 Vinogradarstvo (sadnja i održavanje vinove loze)
* 🍇 Berba i kontrola nivoa šećera (Brix)
* 🍷 Fermentacija i proizvodnja vina
* 📦 Pakovanje vina u palete
* ❄️ Skladištenje u vinskim podrumima
* 🚚 Distribucija i prodaja
* 📑 Evidencija faktura

Svaki podsistem ima **jednu jasno definisanu odgovornost**, a međusobna komunikacija se vrši isključivo preko **interfejsa**.

---

## 👤 Korisnici i autentifikacija

Sistem je zaštićen autentifikacijom. Pre pristupa bilo kojoj funkcionalnosti korisnik se mora prijaviti ili registrovati.

Podržane uloge:

* **Glavni enolog**
* **Kelar majstor**

Uloga korisnika direktno utiče na:

* dostupne menije
* tip skladišnog servisa koji se koristi

---

## 🧱 Arhitektura sistema

Projekat je organizovan po slojevima:

* **Domain** – modeli i interfejsi
* **Services** – poslovna logika
* **Repositories** – rad sa XML bazom podataka
* **Infrastructure** – logger i baza podataka
* **Presentation** – konzolni meniji
* **Tests** – NUnit + Moq testovi

Svi servisi zavise isključivo od **apstrakcija**, što omogućava jednostavnu zamenu implementacija.

---

## 📂 Persistencija podataka

* Podaci se čuvaju u **XML bazi podataka**
* Svaki entitet ima jedinstveni identifikator (GUID)
* Prilikom pokretanja aplikacije učitavaju se **inicijalni testni podaci**

---

## 🌱 Vinova loza

Entitet **VinovaLoza** sadrži:

* Naziv sorte
* Nivo šećera (15.0 – 28.0 Brix)
* Godinu sadnje
* Region uzgoja
* Fazu zrelosti:

  * posađena
  * cveta
  * zrenje
  * spremna za berbu
  * obrana

### Sервис vinogradarstva omogućava:

* Sadnju nove loze
* Promenu nivoa šećera za procenat
* Berbu željenog broja loza

Ako tokom proizvodnje vina nema dovoljno loze, servis automatski zahteva sadnju nove loze sa nasumičnim nivoom šećera.

---

## 🍷 Proizvodnja vina (fermentacija)

Entitet **Vino** sadrži:

* Naziv vina
* Kategoriju:

  * stolno
  * kvalitetno
  * premium
* Zapreminu (0.75 L ili 1.5 L)
* Šifru serije (VN-2025-ID)
* ID vinove loze
* Datum flaširanja

### Logika fermentacije:

* Fermentacija se pokreće na zahtev servisa pakovanja
* Od jedne obrane loze dobija se **1.2 L vina**
* Ako je nivo šećera iznad 24.0 Brix, sistem automatski balansira ukus:

  * zahteva sadnju nove loze
  * smanjuje njen Brix za iznos prekoračenja

---

## 📦 Pakovanje vina

Vina se grupišu u **palete**.

Entitet **Paleta** sadrži:

* Šifru palete
* Adresu odredišta
* ID vinskog podruma
* Listu ID‑eva vina
* Status:

  * upakovana
  * otpremljena

Jedno vino se može nalaziti **isključivo u jednoj paleti**.

Ako ne postoji dostupna upakovana paleta, sistem automatski započinje novo pakovanje.

---

## ❄️ Skladištenje vina

Entitet **VinskiPodrum** sadrži:

* Naziv
* Temperaturu skladištenja
* Maksimalan broj paleta

### Implementacije skladišnog servisa:

* **Vinski podrum** (koristi ga glavni enolog)

  * do 5 paleta po isporuci
  * 0.3 s po paleti

* **Lokalni kelar** (koristi ga kelar majstor)

  * do 2 palete po isporuci
  * 1.8 s po paleti

Odgovarajuća implementacija se bira **dinamički** na osnovu uloge korisnika.

---

## 🚚 Prodaja i distribucija

Sistem nudi katalog svih dostupnih vina.

Kupac bira:

* vrstu vina
* količinu flaša

Nakon toga sistem:

1. Zahteva isporuku paleta iz skladišta
2. Raspakuje palete
3. Kreira **fakturu**

### Faktura sadrži:

* Tip prodaje:

  * restoranska
  * diskont pića
* Način plaćanja:

  * gotovina
  * predračun
  * gotovinski račun
* Spisak vina i količina
* Ukupan iznos

Glavni enolog ima uvid u **sve fakture**.

---

## 📝 Logger servis

Sve značajne akcije u sistemu se evidentiraju putem logger servisa.

Podržani nivoi:

* INFO
* WARNING
* ERROR

Logovi se upisuju u **tekstualnu datoteku** sa datumom, vremenom i opisom događaja.

---

## 🧪 Testiranje

Projekat sadrži automatizovane testove:

* NUnit
* Moq

Obuhvaćeno je:

* najmanje 3 modela
* najmanje 3 servisa

Testovi proveravaju:

* poslovnu logiku
* validnost proračuna
* ponašanje servisa uz mock zavisnosti

---
