# Звіт з аналізу інкапсуляції в Open-Source проєкті

## 1. Обраний проєкт
**Назва:** Newtonsoft.Json (Json.NET)  
**Посилання на GitHub:** [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)  

## 2. Аналіз інкапсуляції

### Клас: JsonProperty
**Посилання на файл:** [JsonProperty.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Serialization/JsonProperty.cs)  
**Опис класу:** Описує метадані властивості JSON при серіалізації та десеріалізації (назва властивості, тип, правила обробки null-значень тощо).  
**Поля:** Приватні поля для збереження внутрішнього стану об'єкта.  
```csharp
private string? _propertyName;
private Type? _propertyType;
```
**Властивості:** Використовуються класичні властивості з геттерами та сетерами для приватних полів, а також автоматичні властивості для прапорів конфігурації.  
```csharp
public string? PropertyName
{
    get => _propertyName;
    set => _propertyName = value;
}

public Type? PropertyType
{
    get => _propertyType;
    set => _propertyType = value;
}

public bool NullValueHandling { get; set; }
public bool DefaultValueHandling { get; set; }
```
**Індексатори/Оператори:** У цьому класі індексатори та перевантажені оператори відсутні.

---

### Клас: JsonSerializerSettings
**Посилання на файл:** [JsonSerializerSettings.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/JsonSerializerSettings.cs)  
**Опис класу:** Містить конфігураційні параметри для налаштування процесу серіалізації та десеріалізації об'єктів.  
**Поля:** Приватні поля для захисту конфігураційних значень від неконтрольованої зміни.  
```csharp
private int? _maxDepth;
private bool _maxDepthSet;
```
**Властивості:** Властивість `MaxDepth` забезпечує доступ до приватного поля і містить валідацію вхідних даних у сетері.  
```csharp
public int? MaxDepth
{
    get => _maxDepth;
    set
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "MaxDepth must be positive.");
        }
        _maxDepth = value;
        _maxDepthSet = true;
    }
}
```
**Індексатори/Оператори:** У цьому класі індексатори та перевантажені оператори відсутні.

---

### Клас: JObject
**Посилання на файл:** [JObject.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Linq/JObject.cs)  
**Опис класу:** Представляє JSON-об'єкт у вигляді ієрархічного дерева (LINQ to JSON) для динамічної маніпуляції полями.  
**Поля:** Приватна колекція членів JSON-об'єкта для внутрішнього збереження пар ключ-значення.  
**Властивості:** Забезпечують доступ до метаданих та колекцій властивостей об'єкта.  
**Індексатори/Оператори:** Використовується перевизначений індексатор для зручного доступу до полів JSON-об'єкта за їхнім іменем:  
```csharp
public override JToken? this[string propertyName]
{
    get
    {
        ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));
        JProperty? property = Property(propertyName);
        return property?.Value;
    }
    set
    {
        JProperty? property = Property(propertyName);
        if (property != null)
        {
            property.Value = value ?? JValue.CreateNull();
        }
        else
        {
            Add(new JProperty(propertyName, value));
        }
    }
}
```

---

## 3. Практики валідації у властивостях

**Приклад 1:** Перевірка допустимого діапазону значень у set-аксесорі з викиданням винятку `ArgumentOutOfRangeException`.  
```csharp
public int? MaxDepth
{
    get => _maxDepth;
    set
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "MaxDepth must be positive.");
        }
        _maxDepth = value;
        _maxDepthSet = true;
    }
}
```

**Приклад 2:** Використання статичного утилітарного класу `ValidationUtils` для централізованої перевірки параметрів на `null`.  
```csharp
ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));
```

**Висновки щодо валідації:**  
Використані підходи дозволяють реалізувати принцип *Fail-Fast* — некоректні дані відсікаються одразу в момент їхньої спроби запису. Використання допоміжних методів валідації запобігає дублюванню однакових `if`-перевірок по всьому коду проєкту.

---

## 4. Загальні висновки
У ході аналізу open-source проєкту Newtonsoft.Json з'ясовано, що принципи інкапсуляції реалізовані на високому рівні. Усі поля є приватними, а зовнішній доступ контролюється властивостями та індексаторами. Перевірки та валідація у властивостях надійно захищають об'єкти від невалідного стану.

---

## Відповіді на контрольні запитання

1. **За якими ознаками можна зробити висновок, що в класі дотримано інкапсуляції?**  
   Усі поля класу оголошені з модифікатором `private`, доступ до них надається виключно через властивості або методи, а в `set`-аксесорах присутня валідація вхідних даних.

2. **Які підходи до валідації властивостей ви зустріли в реальному коді?**  
   Пряма перевірка умов у `set`-аксесорі з викиданням винятків (`ArgumentOutOfRangeException`), а також використання статичних методів утилітарних класів (наприклад, `ValidationUtils.ArgumentNotNull`).

3. **Чому для аналізу важливо наводити посилання на конкретні класи та фрагменти коду?**  
   Це підтверджує практичне проведення аналізу безпосередньо у першоджерелі та дозволяє перевірити відповідність висновків реальному коду.

4. **Які висновки щодо якості проєктування ви зробили після порівняння 2-3 класів?**  
   Професійний C#-код поєднує захист внутрішнього стану через приватні поля з гнучкими та зручними публічними інтерфейсами (властивостями, автоматичними властивостями та індексаторами).
