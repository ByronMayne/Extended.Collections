# Lazy Dictionary 

A dictionary implementation that creates adds new values when a key is accessed that does not exist. 

Pros:
 * **Avoids the need for explicit error handling:** In a traditional dictionary, you typically need to check if a key exists before accessing it to avoid KeyError exceptions. With a custom dictionary, you eliminate the need for such error checks, making your code cleaner and more concise.
 * **Provides default values:** A custom dictionary can automatically generate and return default values when a key is not found. This is especially useful when you want to assign default values to missing keys, saving you from writing conditional logic to handle them.more complex operations to initialize a new key-value pair.
* **Reduces boilerplate code:** Without the need for conditional checks for key existence, your code becomes more concise and less cluttered, which can lead to increased maintainability and fewer opportunities for errors.
  
Cons:
 * **Unintentional Key Creation:** Automatically creating new values for missing keys can lead to unintentional data insertion, which may not always be desirable. It can make it easier to overlook mistakes in your code, such as typos in key names.
* **Hidden Issues:** When a missing key automatically generates a new value, debugging can become more challenging because you might not immediately notice if a key is being used incorrectly or if there's a problem with your data.

### Hypothetical Use Case:
Counting the number of words that start with a letter.

```csharp file=../../../src/Extended.Collections.Playground/Generic/Specialized/LazyDictionarySandbox.cs#L2-
// Imported
```
