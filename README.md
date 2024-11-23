# Naziv projekta

BidBuddy

Projekt izrađen timski u skolpu kolegija **Razvoj programskih proizvoda**

## Opis domene
BidBuddy je desktop aukcijska platforma za javnu prodaju s ograničenim vremenom prodaje u kojoj se roba, rijetki artefakti ili imovina prodaju najvećem ponuditelju. Korisnici platforme BidBuddy će na početnom zaslonu imati pregled aukcija koje će uskoro završiti. Aukcije u ovom pogledu se mogu sortirati po nazivu, trajanju, cijeni, datumu te filtrirati po kategoriji i regiji. Korisnici imaju mogućnost kreiranja svojih vlastitih aukcija gdje definiraju kategoriju svoje aukcije, početnu cijenu, cijenu pod kojom se može odmah odkupiti predmet aukcije te druge stavke aukcije. Korisnici mogu otvorati dodatne informacije o pojedinim aukcijama gdje imaju mogućnost natjecanja za te aukcije. Kada korisnikove aukcije završe aukcije prelaze u stanje “Čekanje uplate” kada korisnik koji pobjeđuje aukciju potvrđuje da je predmet aukcije njemu dostavljen, a vlasnik aukcije potvrđuje da je aukcija isplaćena i tada se smatra aukcija izvršenom. Kada aukcija završi može se pretraživati na stranici “Povijest aukcija”.

## Specifikacija projekta

<table>
  <tr>
   <td>Broj
   </td>
   <td>Naziv
   </td>
   <td>Opis
   </td>
  
  </tr>
  <tr>
   <td>FZ1
   </td>
   <td>Prijava i registracija
   </td>
   <td>Korisnici aplikacije će se morati prijaviti u aplikaciju radi kontrole pristupa. Novi korisnici će se moći registrirati. Postoje dvije uloge korisnika: običan korisnik i admin. Korisnik se može odjaviti.
   </td>
   
  </tr>
  <tr>
   <td>FZ2
   </td>
   <td>Prikaz aukcija<strong> 
 
</strong>
   </td>
   <td>Korisnik može vidjeti aukcije koje su u tijeku. Može pretraživati aukcije prema nazivu. Aukcije se prikazuju kao na skici. Klikom na aukciju otvaraju se detalji o aukciji, može se sortirati aukcije prema parametrima. Korisnik može filtrirati aukcije prema kategorijama, regijama, ako je aukcija od provjerenog korisnika te po korisniku koji je stvorio aukciju. Admin može vidjeti i pretraživati aukcije svih korisnika koje su u tijeku i one koje su završile. Prikazu detalja o aukciji uključuje: naziv aukcije, opis, podatke o prodavaču, stanje aukcije. Prikazuje tijek aukcije (listu svih ponuda drugih korisnika). Prikazuje i sve slike proizvoda na aukciji.
   </td>
  
  </tr>
  <tr>
   <td>FZ3
   </td>
   <td>Prikaz statistike o prethodnim aukcijama
   </td>
   <td>Kod prikaza aukcija koje je korisnik kreirao, prilikom odabira aukcije biti će prikazni tzv. "brza statistika" koja će prikazivati ime aukcije, najveću ponudu i ponuditelja i vrijeme ponude. Nakon završetka aukcije, kreator aukcije moći će izvesti podatke o aukciji u PDF format. PDF dokument će sadržavati sve podatke aukcije, uključujući i tablicu svih sudionika zajedno s njhovim ponudama.
   </td>
 
  </tr>
  <tr>
   <td>FZ4
   </td>
   <td>Upravljanje recenzijama prodavača
   </td>
   <td>Korisnici mogu na aukcijama gdje su pobjednici ocijeniti korisnika prodavača i dati kratku recenziju o tome korisniku. Prodavač može vidjeti sve svoje recenzije, tko ih je napisao i od koje aukcije te recenzije pripadaju. 

   </td>
  
  </tr>
  <tr>
   <td>FZ5
   </td>
   <td>Obavještavanje korisnika o akcijama
   </td>
   <td>Korisnici mogu prilikom dodavanja ponuda odabrati žele li primati obavijest putem e-pošte. Obavijesti će se slati, kada korisnik ponudi svoju ponudu, kada netko drugi ponudi veću ponudu od njegove, te kada aukcija završi i vlasnik aukcija ga obavijesti ako je pobijedio.
   </td>
  
  </tr>
  <tr>
   <td>FZ6
   </td>
   <td>Postavi ponudu za proizvod na aukciji
   </td>
   <td>Ako aukcija još traje, korisnik ima mogućnost postavljanja ponude ili instant kuplje (instant buy opcija). Ako postavimo ponudu na aukciju u zadnju minutu, aukcija će se produljiti za vrijeme definirano od vlasnika aukcije. Korisnicima će biti dostupan tablični popis svih aukcija na koje su postavili ponudu. Popis će sadržavati ime aukcije, vrijednost ponude, te da li su ponudili najveću ponudu na aukciji ili ne.
   </td>
  
  </tr>
  <tr>
   <td>FZ7
   </td>
   <td>Upravljanje aukcijom
   </td>
   <td>Korisnik može stvoriti aukciju nekog proizvoda.
<p>
Potrebno je da odabere kategoriju proizvoda, dodaje sliku, dodaje naziv i opis. Postavlja vrijeme trajanja aukcije. Postavlja početnu cijenu za ponude (minimal bid). Također, korisnik ako želi može postaviti i cijenu za kupnju odmah (instant buy cijena). 
Korisnik može upravljati svojom aukcijom. Na primjer, može promijeniti stanje aukcije (u završeno ili produljiti ako istekne).
   </td>

  </tr>
  <tr>
   <td>FZ8
   </td>
   <td>Upravljanje transakcijama
   </td>
   <td>Nakon što aukcija završi, ona prelazi u stanje čekanja uplate korisnika. Prilikom plaćanja može birati dali želi plaćati Virmansko (plaćanje IBAN-om) ili karticom. Odabirom Virmanskim plaćanjem korisniku se daje IBAN na koji se uplaćuje sredstva i QR kod za skeniranje preko kojeg može platiti nekom platformom online bankarstva, ako se odabere plaćanje karticom korisnika se traže podaci kartice kojom plaća. Ako je transakcija uspješna korisniku koji je bio prodavač aukcije dobiva sredstva.
   </td>
  
  </tr>
  <tr>
   <td>FZ9
   </td>
   <td>Prikaz preporuka za aukcije
   </td>
   <td>Na početnom ekranu kreira se pogled preporučenih aukcija koje sustav bira iz korisnikovih prethodnih sudjelovanja na aukcijama, njegove pretrage te aukcije od prodavatelja od kojih je već kupio neke stvari. Ako je korisnički račun tek stvoren ili ne postoji aukcija po navedenim kriterijima za preporuku onda se korisniku preporučuju najtraženije aukcije.
   </td>
 
  </tr>
  <tr>
   <td>FZ10
   </td>
   <td>Upravljanje podataka korisnika
   </td>
   <td>Korisniku se omogućuje pregled svojeg profila i uređivanje podataka: ime, prezime, email, telefon, iban. Korisnik također može vidjeti aukcije povezane s njime. To uključuje aukcije koje je korisnik kreirao i one na kojima se natjecao i podatke o njima kao što su ako su osvojili aukciju. 

   </td>
  
  </tr>
  <tr>
   <td>FZ11
   </td>
   <td>Upravljanje korisnika<strong> 
</strong>
   </td>
   <td>Admin sustava može vidjeti sve korisnike aplikacije. Admin može korisniku dati ili oduzeti ulogu admina te može dodijeliti oznaku provjerenog korisnika. Kako bi korisnik mogao koristiti aplikaciju, nakon registracije korisnika, admin mora potvrditi registraciju. Admin može i deaktivirati nečiji račun i napisati razlog. 

   </td>
  
  </tr>
  <tr>
   <td>FZ12
   </td>
   <td>Upravljanje oznakama aukcija
   </td>
   <td>Admin može vidjeti, dodavati i uređivati oznake. Oznake uključuju regije i kategorije. Kategorije se mogu granati i na podkategorije.  Regija je lokacija gdje prodavač aukcije dostavlja svoja dobara. Regija je dodatan način filtriranja aukcija tako da korisnik može pronaći aukcije iz željene regije. Admin može vidjeti, dodavati i uređivati kategorije proizvoda koji se mogu staviti na aukciju, svaka kategorija može još imati, ali nije nužna, podkategorije koje pobliže opisuju predmet aukcije.
   </td>
   
  </tr>
</table>

### Stavke u navigaciji

* Aukcije - prikazuje sve aukcije u tijeku
* Moje aukcije - prikazuje aukcije koje smo mi kreirali i još su u tijeku, prikazuje i aukcije za koje smo se počeli natjecati
* Povijest aukcija - prikazuje završene aukcije povezana s nama, a to su završene aukcije koje smo pobijedili te završene aukcije koje smo mi kreirali
* Profil - omogućuje pregled svojeg profila i uređivanje podataka (ime, prezime, email, telefon, iban)

Dodatne stavke u navigaciji za admina:
* Sve aukcije - prikaz svih aukcija u sustavu
* Korisnici - prikaz i upravljanje svim korisnicima
* Postavke - prikaz i uređivanje oznaka (regije, kategorije, podkategorije)

### Skica - pregled aukcija
![skica](https://github.com/foivz/rpp23-project-kjacmenja21-tpapic21-jmojzes21-drusak21/blob/9959969934d63d7ab8492dffe952b2561a19add4/Documentation/Skice/prikaz-aukcija.jpg)

## Tehnologije i oprema

* Windows Forms, .NET Framework 4.8 razvojni okvir
* Github
* SQL baza podataka
* Visual Paradigm 
