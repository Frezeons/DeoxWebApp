# DeoxWebApp

DeoxWebApp, ASP.NET Web Forms tabanlı bir web uygulamasıdır. Kullanıcıların sisteme kaydolmasını, giriş yapmasını ve şifrelerini sıfırlamasını sağlayan temel kimlik doğrulama ve kullanıcı yönetimi işlevlerini içerir.

## 🚀 Proje Özellikleri

Bu proje, modern bir web uygulamasının temelini oluşturan aşağıdaki özelliklere sahiptir:

*   **Kullanıcı Kaydı:**
    *   Yeni kullanıcılar bir kullanıcı adı, e-posta adresi ve şifre ile sisteme kaydolabilirler.
    *   Kullanıcı adı ve şifre için belirli geçerlilik kuralları (minimum uzunluk, karakter gereksinimleri) uygulanır.
    *   Kayıt sırasında kullanıcı adı veya e-postanın daha önce alınıp alınmadığı kontrol edilir.
*   **Kullanıcı Girişi:**
    *   Kayıtlı kullanıcılar, kimlik bilgileriyle sisteme giriş yapabilir.
    *   Giriş yapan kullanıcının e-posta adresini doğrulayıp doğrulamadığı ve yönetici tarafından onaylanıp onaylanmadığı kontrol edilir.
    *   Başarılı girişin ardından kullanıcılar ana uygulama sayfasına yönlendirilir.
*   **E-posta Doğrulaması:**
    *   Kullanıcı kaydolduğunda, hesabını aktifleştirmesi için bir doğrulama e-postası gönderilir.
    *   Kullanıcı giriş yapmaya çalıştığında e-posta doğrulaması yapılmamışsa, doğrulama e-postası yeniden gönderilebilir.
*   **Şifre Sıfırlama:**
    *   Kullanıcılar, şifrelerini unuttuklarında kullanıcı adlarını veya e-posta adreslerini kullanarak sıfırlama talebinde bulunabilirler.
    *   Sisteme kayıtlı e-posta adresine, 30 dakika geçerliliği olan, tek kullanımlık bir şifre sıfırlama bağlantısı gönderilir.
*   **Güvenlik:**
    *   Kullanıcı şifreleri MD5 ile hash'lenerek veritabanında güvenli bir şekilde saklanır.
    *   E-posta doğrulama ve şifre sıfırlama işlemleri için güvenli token'lar oluşturulur.

## 🛠 Kullanılan Teknolojiler

### Back-end (Sunucu Tarafı)
*   **Framework:** ASP.NET Web Forms
*   **Dil:** C#
*   **Veritabanı İşlemleri:** Ham SQL sorguları çalıştıran bir `DataLayer` sınıfı üzerinden veritabanı yönetimi sağlanmaktadır.
*   **Kimlik Doğrulama:** Özel bir `Auth` sınıfı, kullanıcı doğrulama ve oturum yönetimi işlemlerini gerçekleştirir.
*   **E-posta Gönderimi:** `MailManagement` sınıfı, SMTP üzerinden doğrulama ve şifre sıfırlama e-postalarını gönderir.

### Front-end (İstemci Tarafı)
*   **Arayüz & Tasarım:** Projenin kullanıcı arayüzü ve responsive (duyarlı) tasarımı için Bootstrap v5.3.3 kullanılmıştır.
*   **Dinamik İşlemler:** DOM manipülasyonu ve Bootstrap'in JavaScript bileşenleri için jQuery v3.7.1 projeye dahil edilmiştir.

## 📂 Proje Dosya Yapısı ve Açıklamaları

Projenin ana mantığı aşağıdaki C# dosyalarında yer almaktadır:

*   `Register.aspx.cs`: Yeni kullanıcı kayıt işlemlerini yönetir. Kullanıcı adı, e-posta ve şifre için temel doğrulamaları yapar ve başarılı kayıt sonrası doğrulama e-postası gönderimini tetikler.
*   `Login.aspx.cs`: Kullanıcı giriş işlemlerini yönetir. Başarılı kimlik doğrulamanın ardından kullanıcının e-posta ve yönetici onay durumunu kontrol ederek yönlendirme yapar. Eğer doğrulama eksikse, kullanıcıyı bilgilendirir ve gerekirse yeni doğrulama e-postası gönderir.
*   `Reset_Password.aspx.cs`: Şifre sıfırlama sürecini yönetir. Hem kullanıcıdan talep alan arayüzün kodunu içerir hem de istemciden gelen AJAX isteklerini işleyerek şifre sıfırlama e-postasını gönderir.
*   `Models/`: Bu klasörde `Register.cs`, `Auth.cs`, `DataLayer.cs`, `MailManagement.cs` gibi iş mantığını ve veri erişimini soyutlayan sınıflar bulunur.
