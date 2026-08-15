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
| Giriş Yap | Kayıt Ol |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/6ae075d0-c886-4b87-8eef-e1441496b40a" width="450" /> | <img src="https://github.com/user-attachments/assets/d5799949-a150-4653-9cf2-0d740a318ee4" width="450" /> |

| Şifremi Unuttum | E-Posta Onayı |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/920eb6c4-7f36-482e-acd1-d6856915e944" width="450" /> | <img src="https://github.com/user-attachments/assets/f83384df-f17b-4807-b6c0-8be7692834f5" width="450" /> |

---

### Mesajlaşma & Detay Ekranları
| Yeni Mesaj Oluşturma | Mesaj Detayı & Yanıtlama |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/1e4fb1f5-39a0-493f-990e-11adfb5cf063" width="450" /> | <img src="https://github.com/user-attachments/assets/afeca236-f514-4d34-878d-f4dc6d0558e7" width="450" /> |

---

### Gelen Kutusu & Klasörler
| Gelen Kutusu | Gönderilen Mesajlar |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/bc2bdaec-8c7c-4af3-90bb-2970b02b9111" width="450" /> | <img src="https://github.com/user-attachments/assets/51719aa9-74cb-412a-a778-0cc1bd9028c0" width="450" /> |

| Yıldızlı Mesajlar | Çöp Kutusu |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/ec3ef032-589b-4a97-b8c7-5c48bfa6f011" width="450" /> | <img src="https://github.com/user-attachments/assets/8583b45e-207c-4e57-b677-75f7e1bee575" width="450" /> |

---

### Admin Yönetim Paneli
| Dashboard & İstatistikler | Kullanıcı Yönetimi |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/77b044d3-00a8-4c71-a34b-1cb6010283fa" width="450" /> | <img src="https://github.com/user-attachments/assets/870c8d86-017e-4fb9-9519-882c37831663" width="450" /> |

| Kategori Yönetimi | Şikayet Edilen Mesaj Detayı |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/f7f4fa61-0359-4581-8707-6bf66897be04" width="450" /> | <img src="https://github.com/user-attachments/assets/211b5e17-0d0d-430d-aa0f-c3bbb6991d06" width="450" /> |

---

### Hata & Yetki Ekranları
| 404 Sayfa Bulunamadı | 403 Erişim Reddedildi |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/d6e0e7a1-b83d-4e4d-a22f-9fedec5bbef0" width="450" /> | <img src="https://github.com/user-attachments/assets/ac7b7221-f0a3-4df7-a804-37d169c86a93" width="450" /> |

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
