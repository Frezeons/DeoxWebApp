# DeoxWebApp

> Kayıt, giriş, e-posta doğrulama ve şifre sıfırlama sunan ASP.NET Web Forms kullanıcı yönetimi uygulaması.

**Durum:** okul projesi · **Stack:** ASP.NET Web Forms · C# · SQL (ham sorgu) · Bootstrap 5.3 · jQuery 3.7

## Ne yapar
- Kullanıcı adı / e-posta / şifre ile kayıt; uzunluk ve karakter kuralları, kullanıcı adı/e-posta tekrar kontrolü
- Giriş sırasında e-posta doğrulaması ve yönetici onayı kontrolü
- Kayıtta doğrulama e-postası; doğrulanmamış hesaba yeniden gönderim
- Kullanıcı adı veya e-postayla şifre sıfırlama; 30 dakika geçerli, tek kullanımlık bağlantı (AJAX)
- SMTP üzerinden e-posta gönderimi (`MailManagement`)

## Dizin yapısı
```
Register.aspx(.cs)        # kayıt
Login.aspx(.cs)           # giriş + doğrulama/onay kontrolü
Reset_Password.aspx(.cs)  # şifre sıfırlama (AJAX uçları dahil)
Models/                   # Register, Auth, DataLayer, MailManagement
```

## Notlar
- Şifreler MD5 ile hash'leniyor — günümüz için güvenli değildir; gerçek kullanımda bcrypt/PBKDF2'ye geçilmeli.
- Veri erişimi `DataLayer` sınıfındaki ham SQL sorgularıyla yapılır.
