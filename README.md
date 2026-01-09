# SudaScan
ربط أجهزة الماسح الضوئي (Scanner) بتطبيقات الويب محليًا

## نبذة عن المشروع
SudaScan هو تطبيق محلي (Local Agent) يعمل على نظام Windows، يهدف إلى حل مشكلة ربط أجهزة المسح الضوئي مع تطبيقات الويب مباشرة من المتصفح دون الحاجة إلى تحميل الملفات يدويًا أو استخدام حلول خارجية أو خدمات مدفوعة.

التطبيق يعمل بالكامل محليًا، ويتعامل مع جهاز Scanner عبر مكتبة WIA، ويعرض واجهة ويب بسيطة بالإضافة إلى واجهة برمجية (HTTP API) يمكن لأي نظام آخر استهلاكها بسهولة.

الفكرة الأساسية هي:
- المستخدم يضغط زر "مسح" من داخل المتصفح أو أي نظام
- يتم تنفيذ عملية المسح عبر جهاز Scanner
- يظهر الملف مباشرة داخل النموذج (Form) أو يتم تنزيله تلقائيًا
- بدون رفع ملفات يدوي
- بدون إنترنت
- بدون خدمات خارجية

---

## الخصائص الرئيسية
- يعمل محليًا (Localhost)
- لا يحتاج إلى اتصال إنترنت
- لا يعتمد على خدمات مدفوعة أو محجوبة
- واجهة ويب عربية بسيطة
- يدعم الإخراج بصيغ PNG و PDF
- واجهة API قابلة للاستهلاك من أي تطبيق
- خفيف وسهل التشغيل
- قابل للدمج في أي نظام ويب أو Desktop

---

## المتطلبات التقنية
- نظام تشغيل Windows 10 أو أحدث
- .NET 8 Runtime
- جهاز Scanner يدعم WIA
- متصفح حديث (Chrome, Edge, Firefox)

---

## طريقة التثبيت
1. قم بتنزيل ملف التثبيت (Setup.exe)
2. شغّل ملف التثبيت واتبع خطوات التثبيت
3. بعد الانتهاء، سيتم إنشاء اختصار للتطبيق
4. شغّل التطبيق من الاختصار

---

## طريقة التشغيل
- عند تشغيل التطبيق، يعمل كخدمة محلية
- يفتح المتصفح تلقائيًا على العنوان:
  http://localhost:5050
- ضع المستند داخل جهاز Scanner
- اختر صيغة الإخراج (PNG أو PDF)
- اضغط زر "مسح الملف"
- سيظهر الملف مباشرة داخل الصفحة أو يتم تنزيله

---

## واجهة برمجة التطبيقات (API)
يوفر SudaScan واجهة HTTP بسيطة يمكن لأي نظام استهلاكها.

### نقطة النهاية الأساسية
POST  
http://localhost:5050/scan

### المعاملات
- format  
  - png : إخراج صورة
  - pdf : إخراج ملف PDF

---

## مثال استهلاك الـ API من تطبيق ويب (JavaScript)
```javascript
fetch("http://localhost:5050/scan?format=png", {
    method: "POST"
})
.then(response => response.blob())
.then(blob => {
    const img = document.createElement("img");
    img.src = URL.createObjectURL(blob);

مثال استهلاك الـ API من تطبيق سطح مكتب (C#)
using System.Net.Http;

var client = new HttpClient();
var response = await client.PostAsync(
    "http://localhost:5050/scan?format=pdf",
    null
);

var fileBytes = await response.Content.ReadAsByteArrayAsync();
File.WriteAllBytes("scan.pdf", fileBytes);

المعمارية العامة

يتكون الحل من:

جهاز Scanner

تطبيق SudaScan المحلي (EXE)

واجهة Web UI

واجهة HTTP API

أي تطبيق ويب أو Desktop يستهلك الـ API

جميع المكونات تعمل محليًا على نفس الجهاز.

قابلية الدمج

يمكن دمج SudaScan مع:

أنظمة ويب (ASP.NET, PHP, Node.js, Java, Python)

أنظمة Desktop

أنظمة حكومية أو مؤسسية تعمل محليًا

أي نظام يدعم HTTP Requests
    document.body.appendChild(img);
});
