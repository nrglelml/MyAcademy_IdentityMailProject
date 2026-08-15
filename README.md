# 📨 NMail — ASP.NET Core Identity Mesajlaşma & E-Posta Yönetim Sistemi

<p align="center">
  <b>ASP.NET Core MVC</b> ve <b>ASP.NET Core Identity</b> kullanılarak geliştirilmiş; rol tabanlı yetkilendirme, klasörleme mimarisi, dinamik filtreleme ve gelişmiş bir admin yönetim paneli barındıran kurumsal mesajlaşma uygulaması.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0%20%2F%209.0-512BD4?style=flat-square&logo=dotnet" alt=".NET" />
  <img src="https://img.shields.io/badge/Entity%20Framework%20Core-Code%20First-512BD4?style=flat-square" alt="EF Core" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-Identity-blue?style=flat-square" alt="Identity" />
  <img src="https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=flat-square&logo=tailwind-css&logoColor=white" alt="Tailwind CSS" />
  <img src="https://img.shields.io/badge/MailKit-SMTP-orange?style=flat-square" alt="MailKit" />
</p>

---

## 📌 İçindekiler
- [Öne Çıkan Özellikler](#-öne-çıkan-özellikler)
- [Kullanılan Teknolojiler](#-kullanılan-teknolojiler)
- [Mimari Kararlar](#-mimari-kararlar)
- [Proje Ekran Görüntüleri](#-proje-ekran-görüntüleri)
- [Proje Dizin Yapısı](#-proje-dizin-yapısı)
- [Kurulum ve Başlangıç](#-kurulum-ve-başlangıç)
- [Bilinen Sınırlamalar](#-bilinen-sınırlamalar)

---

## 🚀 Öne Çıkan Özellikler

### 🔐 Kimlik Doğrulama & Profil Yönetimi
- **Hesap İşlemleri:** Kayıt ol, giriş yap, güvenli çıkış yap.
- **E-Posta Doğrulama:** SMTP üzerinden onay bağlantısı ve tekrar gönderme desteği.
- **Şifre Sıfırlama:** Token tabanlı şifremi unuttum ve güvenli parola yenileme akışı.
- **Profil & Güvenlik:** Ad, soyad, profil fotoğrafı güncelleme ve eski şifre doğrulamalı parola değiştirme.
- **Erişim Kontrolü:** Tekil e-posta zorunluluğu ve yetkisiz erişimler için özel hata ekranları.

### ✉️ Mesajlaşma & Klasör Yönetimi
- **Akıllı Alıcı Arama:** Mesaj oluştururken kayıtlı ve aktif kullanıcıları anlık autocomplete ile bulma.
- **Konuşma Zinciri (Thread):** Mesaj detayında geçmişi görüntüleme ve zincirleme yanıtlama.
- **Okunma Takibi:** Mesaj açıldığında otomatik okundu işaretleme ve gönderici tarafında durum takibi.
- **Klasörleme:** Gelen Kutusu, Gönderilenler, Taslaklar, Yıldızlı Mesajlar ve Çöp Kutusu.
- **Gelişmiş Filtreleme:** Arama terimi, kategori, okundu/okunmadı durumu ve tarih aralığına göre çok kriterli filtreleme.
- **Şikayet Sistemi:** Kural dışı mesajları gerekçe belirterek moderasyon için bildirme.

### 🛡️ Admin & Moderasyon Paneli
- **Dashboard & Analitik:** Toplam/aktif kullanıcı sayısı, günlük/toplam mesaj hacmi, okunmamış mesajlar ve çöp kutusu istatistikleri.
- **Kullanım Metrikleri:** En aktif mesajlaşan kullanıcılar ve popüler kategori dağılımları.
- **Kullanıcı Moderasyonu:** Kullanıcı listeleme, arama, rol atama (`Admin`/`User`) ve pasife alma (soft-delete).
- **Kategori Taksonomisi:** Sistem genelinde geçerli kategori tanımlama, renk kodu ve etiket yönetimi.
- **Şikayet İnceleme:** Raporlanan mesajları, gönderen/şikayet eden tarafları ve gerekçeleri detaylı inceleme veya silme.

---

## 🛠 Kullanılan Teknolojiler

| Alan | Teknoloji / Kütüphane |
| :--- | :--- |
| **Framework** | ASP.NET Core MVC |
| **Kimlik & Yetkilendirme** | ASP.NET Core Identity |
| **ORM & Veritabanı** | Entity Framework Core (Code-First), MS SQL Server |
| **E-Posta Altyapısı** | MailKit & MimeKit (SMTP Entegrasyonu) |
| **Arayüz (UI)** | Tailwind CSS, Inter Font Ailesi |
| **İkon Seti** | Google Material Symbols Outlined |

---

## 🧠 Mimari Kararlar

- **`MessageFolder` Ayrımı:** Mesajların okundu, yıldızlı veya silinme durumları kullanıcıya özeldir (gönderen ve alıcı kopyaları bağımsızdır). Bu veriler `UserMessage` yerine `MessageFolder` tablosunda çok-a-çok ilişki ile yönetilir.
- **Ayrık `Draft` Varlığı:** Taslaklar gerçek mesajlardan tamamen ayrı bir tabloda tutulur. Böylece `UserMessage.ReceiverId` alanı her zaman zorunlu kalır; `UserMessages` tablosundaki her kayıt kesinlikle gönderilmiş geçerli bir mesajdır.
- **Global Kategori Yapısı:** Kategoriler kullanıcı bazlı değil, tutarlılık adına yalnızca Admin tarafından merkezi olarak tanımlanır.
- **Soft-Delete Yaklaşımı:** Foreign Key (`Restrict`) kısıtlamaları ve veri bütünlüğü gereği kullanıcılar tamamen silinmez; `IsActive` bayrağı üzerinden pasife alınır.
- **İki Kademeli Şikayet Sistemi:** Hızlı filtreleme için `UserMessage.IsReported` bayrağı kullanılırken, gerekçe ve bildiren kullanıcı bilgileri bağımsız `Report` tablosunda saklanır.

---

## 📸 Proje Ekran Görüntüleri

### Kimlik Doğrulama & Hesap Ekranları
<img width="1047" height="910" alt="Ekran görüntüsü 2026-08-15 131141" src="https://github.com/user-attachments/assets/6ae075d0-c886-4b87-8eef-e1441496b40a" />

<img width="1303" height="512" alt="Ekran görüntüsü 2026-08-15 132121" src="https://github.com/user-attachments/assets/d5799949-a150-4653-9cf2-0d740a318ee4" />

<img width="1082" height="879" alt="Ekran görüntüsü 2026-08-15 134318" src="https://github.com/user-attachments/assets/920eb6c4-7f36-482e-acd1-d6856915e944" />

<img width="934" height="823" alt="Ekran görüntüsü 2026-08-15 134427" src="https://github.com/user-attachments/assets/f83384df-f17b-4807-b6c0-8be7692834f5" />

<img width="1022" height="785" alt="image" src="https://github.com/user-attachments/assets/4fadfd1f-60b0-4ba1-83af-cca89b41b891" />

<img width="1180" height="701" alt="image" src="https://github.com/user-attachments/assets/f4d506bb-139a-4a50-b268-26677c6a09a5" />

<img width="1274" height="646" alt="image" src="https://github.com/user-attachments/assets/f012f81c-cde0-499c-89b9-9d398ab546f6" />

<img width="1482" height="880" alt="image" src="https://github.com/user-attachments/assets/8e5d1b2d-a890-4c87-a106-d1ef9a02a849" />

---

### Mesajlaşma & Detay Ekranları

<img width="1179" height="815" alt="Ekran görüntüsü 2026-08-15 134502" src="https://github.com/user-attachments/assets/1e4fb1f5-39a0-493f-990e-11adfb5cf063" />

<img width="1914" height="831" alt="Ekran görüntüsü 2026-08-15 135841" src="https://github.com/user-attachments/assets/afeca236-f514-4d34-878d-f4dc6d0558e7" />

<img width="1919" height="876" alt="Ekran görüntüsü 2026-08-15 135901" src="https://github.com/user-attachments/assets/c7fa838b-60e7-4ce5-a187-d388b1b1fd5c" />

---

### Gelen Kutusu & Klasörler
<img width="1919" height="890" alt="Ekran görüntüsü 2026-08-15 140632" src="https://github.com/user-attachments/assets/bc2bdaec-8c7c-4af3-90bb-2970b02b9111" />

<img width="1919" height="886" alt="Ekran görüntüsü 2026-08-15 140645" src="https://github.com/user-attachments/assets/51719aa9-74cb-412a-a778-0cc1bd9028c0" />

<img width="1917" height="924" alt="Ekran görüntüsü 2026-08-15 140708" src="https://github.com/user-attachments/assets/ec3ef032-589b-4a97-b8c7-5c48bfa6f011" />

<img width="1919" height="903" alt="Ekran görüntüsü 2026-08-15 140825" src="https://github.com/user-attachments/assets/8583b45e-207c-4e57-b677-75f7e1bee575" />

<img width="1808" height="808" alt="Ekran görüntüsü 2026-08-15 141841" src="https://github.com/user-attachments/assets/0a884fa9-8c38-401a-9b5b-64c4b2fe7eaa" />

<img width="1919" height="876" alt="Ekran görüntüsü 2026-08-15 135652" src="https://github.com/user-attachments/assets/b696d4f3-5f98-489c-8853-f6242b6a942a" />

<img width="1919" height="931" alt="Ekran görüntüsü 2026-08-15 140655" src="https://github.com/user-attachments/assets/5bff0322-9c3b-4527-bd16-0f2d20ccbfe5" />

<img width="1910" height="885" alt="Ekran görüntüsü 2026-08-15 140744" src="https://github.com/user-attachments/assets/46aa8def-e038-44d0-9978-ecf9bf218bd4" />

---
### Ayarlar & Şifre Güncelleme 

<img width="1919" height="892" alt="Ekran görüntüsü 2026-08-15 135917" src="https://github.com/user-attachments/assets/a92d95b2-de68-487f-b7b0-b549089dd0ec" />

<img width="1908" height="906" alt="Ekran görüntüsü 2026-08-15 135920" src="https://github.com/user-attachments/assets/8301a502-82d3-434b-a33d-3ce3b3ac5a27" />

### Admin Yönetim Paneli
<img width="1853" height="916" alt="Ekran görüntüsü 2026-08-15 141912" src="https://github.com/user-attachments/assets/77b044d3-00a8-4c71-a34b-1cb6010283fa" />

<img width="1870" height="875" alt="Ekran görüntüsü 2026-08-15 141917" src="https://github.com/user-attachments/assets/870c8d86-017e-4fb9-9519-882c37831663" />

<img width="1859" height="856" alt="Ekran görüntüsü 2026-08-15 141925" src="https://github.com/user-attachments/assets/f7f4fa61-0359-4581-8707-6bf66897be04" />

<img width="1875" height="908" alt="Ekran görüntüsü 2026-08-15 141934" src="https://github.com/user-attachments/assets/9e5d3d6a-f34a-45e0-8b2d-d4a7ef9e65d0" />

<img width="1454" height="836" alt="Ekran görüntüsü 2026-08-15 141939" src="https://github.com/user-attachments/assets/bda9bc71-b1fb-4278-814e-6f1680d3d490" />

<img width="1850" height="876" alt="Ekran görüntüsü 2026-08-15 141944" src="https://github.com/user-attachments/assets/211b5e17-0d0d-430d-aa0f-c3bbb6991d06" />

<img width="1873" height="905" alt="Ekran görüntüsü 2026-08-15 141950" src="https://github.com/user-attachments/assets/dcfec55b-dad4-40db-b527-b8bb8ffeb77f" />

---

### Hata & Yetki Ekranları
<img width="1872" height="986" alt="Ekran görüntüsü 2026-08-15 135122" src="https://github.com/user-attachments/assets/d6e0e7a1-b83d-4e4d-a22f-9fedec5bbef0" />

<img width="1918" height="959" alt="Ekran görüntüsü 2026-08-15 135756" src="https://github.com/user-attachments/assets/ac7b7221-f0a3-4df7-a804-37d169c86a93" />

---

## 📂 Proje Dizin Yapısı

```plaintext
NMail/
├── Areas/
│   ├── User/                  # Mesajlaşma, klasörler, taslaklar ve profil modülleri
│   └── Admin/                 # Kullanıcı yönetimi, kategori taksonomisi ve dashboard
├── Entities/                  # AppUser, UserMessage, MessageFolder, Draft, Category, Report...
├── Dtos/                      # Veri transfer nesneleri (Compose, Filter, Profile, Category DTOs)
├── ViewModels/                # Detay sayfaları ve analitik dashboard modelleri
├── Services/                  # MessageService, EmailSender (MailKit SMTP)
├── Context/                   # AppDbContext (EF Core Fluent API yapılandırmaları)
├── Data/                      # SeedData (Varsayılan roller ve sistem yöneticisi oluşturma)
└── Views/                     # Paylaşılan Layout bileşenleri ve ViewComponent'lar
