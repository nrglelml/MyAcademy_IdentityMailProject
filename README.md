NMail — ASP.NET Core Identity Mesajlaşma Sistemi

ASP.NET Core MVC ve Identity kullanılarak geliştirilmiş, rol tabanlı yetkilendirmeye sahip kurumsal içi mesajlaşma (mailleşme) uygulaması. Kullanıcılar arası mesaj gönderme, taslak yönetimi, kategorileme, arama/filtreleme ve bir admin panelinden sistem yönetimi özelliklerini içerir.

İçindekiler
Özellikler
Kullanılan Teknolojiler
Mimari Notlar
Kurulum
Varsayılan Admin Hesabı
Proje Yapısı
Bilinen Sınırlamalar
Özellikler
Kimlik Doğrulama ve Profil
Kayıt ol / giriş yap / çıkış yap
E-posta doğrulama (mail üzerinden onay linki, tekrar gönderme desteği)
Şifremi unuttum / şifre sıfırlama
Profil düzenleme (ad, soyad, profil fotoğrafı)
Şifre değiştirme
Aynı e-posta ile birden fazla hesap açılamaz
Mesaj ekranlarına yalnızca giriş yapmış kullanıcılar erişebilir
Mesajlaşma
Yeni mesaj oluşturma (alıcı autocomplete ile kayıtlı kullanıcı arama)
Yalnızca kayıtlı ve aktif kullanıcılara mesaj gönderimi
Mesaj yanıtlama (konuşma zinciri / thread görünümü)
Mesaj açılınca otomatik okundu işaretleme
Gönderilen mesajın okunma durumunun görüntülenmesi
Mesajları kategorilere atama
Taslak kaydetme, düzenleme, taslaktan gönderme, taslak silme
Mesaj şikayet etme (admin incelemesi için)
Gelen Kutusu / Gönderilenler
Okunan/okunmayan filtresi
Yıldız ile önemli işaretleme
Yıldızlı Mesajlar ekranı
Çöp Kutusuna taşıma ve geri yükleme
Gönderen/alıcı adına, konuya, tarih aralığına ve kategoriye göre arama/filtreleme (birlikte kullanılabilir)
Sayfalama ve yeni/eski sıralama
Rol Yönetimi
User: mesaj gönderme, görüntüleme, profil düzenleme
Admin: kullanıcı listeleme/arama, kullanıcı aktif/pasif yapma, rol atama, kategori yönetimi, sistem istatistikleri, şikayet edilen mesajları inceleme
Admin Paneli
Toplam / aktif kullanıcı sayısı
Toplam ve bugün gönderilen mesaj sayısı
Okunmamış mesaj sayısı
Çöp kutusundaki mesaj sayısı
En fazla mesaj gönderen kullanıcılar
En çok kullanılan kategoriler (kategoriler admin tarafından merkezi olarak yönetilir)
Kullanılan Teknolojiler
ASP.NET Core MVC (.NET)
ASP.NET Core Identity — kimlik doğrulama ve rol yönetimi
Entity Framework Core — SQL Server ile Code First yaklaşımı
MailKit — SMTP üzerinden e-posta gönderimi
Tailwind CSS — arayüz tasarımı
-- Proje Görselleri --
Kimlik Doğrulama / Auth Ekranları

<img width="1047" height="910" alt="Ekran görüntüsü 2026-08-15 131141" src="https://github.com/user-attachments/assets/6ae075d0-c886-4b87-8eef-e1441496b40a" />
<img width="1303" height="512" alt="Ekran görüntüsü 2026-08-15 132121" src="https://github.com/user-attachments/assets/d5799949-a150-4653-9cf2-0d740a318ee4" />
<img width="1082" height="879" alt="Ekran görüntüsü 2026-08-15 134318" src="https://github.com/user-attachments/assets/920eb6c4-7f36-482e-acd1-d6856915e944" />
<img width="934" height="823" alt="Ekran görüntüsü 2026-08-15 134427" src="https://github.com/user-attachments/assets/f83384df-f17b-4807-b6c0-8be7692834f5" />
<img width="1022" height="785" alt="image" src="https://github.com/user-attachments/assets/4fadfd1f-60b0-4ba1-83af-cca89b41b891" />
<img width="1180" height="701" alt="image" src="https://github.com/user-attachments/assets/f4d506bb-139a-4a50-b268-26677c6a09a5" />
<img width="1274" height="646" alt="image" src="https://github.com/user-attachments/assets/f012f81c-cde0-499c-89b9-9d398ab546f6" />
<img width="1482" height="880" alt="image" src="https://github.com/user-attachments/assets/8e5d1b2d-a890-4c87-a106-d1ef9a02a849" />

Mesaj Gönderme / Yanıtlama Ekranları

<img width="1179" height="815" alt="Ekran görüntüsü 2026-08-15 134502" src="https://github.com/user-attachments/assets/1e4fb1f5-39a0-493f-990e-11adfb5cf063" />
<img width="1914" height="831" alt="Ekran görüntüsü 2026-08-15 135841" src="https://github.com/user-attachments/assets/afeca236-f514-4d34-878d-f4dc6d0558e7" />
<img width="1919" height="876" alt="Ekran görüntüsü 2026-08-15 135901" src="https://github.com/user-attachments/assets/c7fa838b-60e7-4ce5-a187-d388b1b1fd5c" />

Gelen Kutusu / Gönderilen Mesajlar / Yıldızlı Mesajlar / Taslaklar / Çöp Kutusu Ekranları

<img width="1919" height="890" alt="Ekran görüntüsü 2026-08-15 140632" src="https://github.com/user-attachments/assets/bc2bdaec-8c7c-4af3-90bb-2970b02b9111" />
<img width="1919" height="886" alt="Ekran görüntüsü 2026-08-15 140645" src="https://github.com/user-attachments/assets/51719aa9-74cb-412a-a778-0cc1bd9028c0" />
<img width="1917" height="924" alt="Ekran görüntüsü 2026-08-15 140708" src="https://github.com/user-attachments/assets/ec3ef032-589b-4a97-b8c7-5c48bfa6f011" />
<img width="1919" height="903" alt="Ekran görüntüsü 2026-08-15 140825" src="https://github.com/user-attachments/assets/8583b45e-207c-4e57-b677-75f7e1bee575" />
<img width="1808" height="808" alt="Ekran görüntüsü 2026-08-15 141841" src="https://github.com/user-attachments/assets/0a884fa9-8c38-401a-9b5b-64c4b2fe7eaa" />
<img width="1919" height="876" alt="Ekran görüntüsü 2026-08-15 135652" src="https://github.com/user-attachments/assets/b696d4f3-5f98-489c-8853-f6242b6a942a" />
<img width="1919" height="931" alt="Ekran görüntüsü 2026-08-15 140655" src="https://github.com/user-attachments/assets/5bff0322-9c3b-4527-bd16-0f2d20ccbfe5" />
<img width="1910" height="885" alt="Ekran görüntüsü 2026-08-15 140744" src="https://github.com/user-attachments/assets/46aa8def-e038-44d0-9978-ecf9bf218bd4" />

Ayarlar / Şifre Güncelleme Ekranları

<img width="1919" height="892" alt="Ekran görüntüsü 2026-08-15 135917" src="https://github.com/user-attachments/assets/a92d95b2-de68-487f-b7b0-b549089dd0ec" />
<img width="1908" height="906" alt="Ekran görüntüsü 2026-08-15 135920" src="https://github.com/user-attachments/assets/8301a502-82d3-434b-a33d-3ce3b3ac5a27" />

Admin Sayfaları

<img width="1853" height="916" alt="Ekran görüntüsü 2026-08-15 141912" src="https://github.com/user-attachments/assets/77b044d3-00a8-4c71-a34b-1cb6010283fa" />
<img width="1870" height="875" alt="Ekran görüntüsü 2026-08-15 141917" src="https://github.com/user-attachments/assets/870c8d86-017e-4fb9-9519-882c37831663" />
<img width="1859" height="856" alt="Ekran görüntüsü 2026-08-15 141925" src="https://github.com/user-attachments/assets/f7f4fa61-0359-4581-8707-6bf66897be04" />
<img width="1875" height="908" alt="Ekran görüntüsü 2026-08-15 141934" src="https://github.com/user-attachments/assets/9e5d3d6a-f34a-45e0-8b2d-d4a7ef9e65d0" />
<img width="1454" height="836" alt="Ekran görüntüsü 2026-08-15 141939" src="https://github.com/user-attachments/assets/bda9bc71-b1fb-4278-814e-6f1680d3d490" />
<img width="1850" height="876" alt="Ekran görüntüsü 2026-08-15 141944" src="https://github.com/user-attachments/assets/211b5e17-0d0d-430d-aa0f-c3bbb6991d06" />
<img width="1873" height="905" alt="Ekran görüntüsü 2026-08-15 141950" src="https://github.com/user-attachments/assets/dcfec55b-dad4-40db-b527-b8bb8ffeb77f" />

Error / Erişim Yetki Hata Ekranları

<img width="1872" height="986" alt="Ekran görüntüsü 2026-08-15 135122" src="https://github.com/user-attachments/assets/d6e0e7a1-b83d-4e4d-a22f-9fedec5bbef0" />
<img width="1918" height="959" alt="Ekran görüntüsü 2026-08-15 135756" src="https://github.com/user-attachments/assets/ac7b7221-f0a3-4df7-a804-37d169c86a93" />

Mimari Notlar

Bu proje boyunca alınan bazı kasıtlı mimari kararlar:

MessageFolder ayrı bir tablo: Bir mesajın "okundu/yıldızlı/silindi" durumu kullanıcıya göre değişebildiği için (gönderenin kendi kopyası ile alıcının kopyası bağımsızdır), bu bilgi UserMessage yerine ayrı bir MessageFolder tablosunda tutulur.
Draft ayrı bir entity: Taslaklar, gerçek mesajlardan (UserMessage) tamamen ayrı bir tabloda tutulur. Böylece UserMessage.ReceiverId her zaman zorunlu kalır — bir satır UserMessages tablosundaysa kesinlikle gönderilmiş, geçerli bir alıcısı olan gerçek bir mesajdır.
Kategoriler global: Kategoriler kullanıcı bazlı değil, yalnızca Admin tarafından yönetilen ortak bir taksonomidir.
Kullanıcılar silinmez, pasif yapılır: Foreign key ilişkileri (Restrict) nedeniyle bir kullanıcı mesaj geçmişi olduğu sürece veritabanından gerçekten silinemez; bunun yerine IsActive alanıyla soft-delete uygulanır.
Şikayet sistemi iki katmanlı: UserMessage.IsReported hızlı filtreleme için bir bayrak, asıl şikayet detayı (kim, ne zaman, neden) ayrı Report tablosunda tutulur.

Proje Yapısı
├── Areas/
│   ├── User/           # Mesajlaşma, profil, taslak ekranları
│   └── Admin/           # Kullanıcı yönetimi, kategori yönetimi, dashboard
├── Entities/             # AppUser, UserMessage, MessageFolder, Draft, Category, Report ...
├── Dtos/                 # Compose, kullanıcı listesi, mesaj listesi DTO'ları
├── ViewModels/           # Mesaj detay, dashboard istatistik view model'leri
├── Services/             # IMessageService / MessageService, IEmailSender / EmailSender
├── Context/              # AppDbContext
├── Data/                 # SeedData (rol + admin oluşturma)
└── Views/
Bilinen Sınırlamalar
Zengin metin editörü (kalın/italik/liste) arayüzde görünür ama henüz işlevsel değil.
Mesaj eki (attachment) desteği yok.
Rol atama şu an yalnızca tek rol (Admin/User) üzerinden çalışır, çoklu rol desteklenmiyor.
E-posta gönderimi geliştirme ortamında Mailtrap ile test edilmiştir; üretim için gerçek bir SMTP sağlayıcısı yapılandırılmalıdır.
