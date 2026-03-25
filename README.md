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
*   **Arayüz & Tasarım:** Projenin kullanıcı arayüzü ve responsive (duy
