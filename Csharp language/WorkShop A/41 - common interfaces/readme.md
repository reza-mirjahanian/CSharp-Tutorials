

| Interface | Namespace | Purpose | Key Members |
|-----------|-----------|---------|-------------|
| `IEnumerable<T>` | System.Collections.Generic | Enables iteration over a collection | `GetEnumerator()` |
| `ICollection<T>` | System.Collections.Generic | Represents a generic collection | `Add()`, `Remove()`, `Clear()`, `Count`, `Contains()` |
| `IList<T>` | System.Collections.Generic | Indexable collection | `Insert()`, `RemoveAt()`, `IndexOf()`, indexer `[int]` |
| `IDictionary<TKey, TValue>` | System.Collections.Generic | Key-value pair collection | `Add()`, `Remove()`, `ContainsKey()`, `Keys`, `Values` |
| `IComparable<T>` | System | Defines comparison method for sorting | `CompareTo(T)` |
| `IEquatable<T>` | System | Defines equality comparison | `Equals(T)` |
| `IDisposable` | System | Releases unmanaged resources | `Dispose()` |
| `IEnumerator<T>` | System.Collections.Generic | Supports iteration | `Current`, `MoveNext()`, `Reset()` |
| `IComparer<T>` | System.Collections.Generic | Custom comparison logic | `Compare(T, T)` |
| `IObservable<T>` | System | Observable pattern for push-based notifications | `Subscribe(IObserver<T>)` |
| `IObserver<T>` | System | Observer pattern for receiving notifications | `OnNext(T)`, `OnError()`, `OnCompleted()` |
| `IQueryable<T>` | System.Linq | Queryable data source (LINQ) | `Provider`, `Expression`, inherits `IEnumerable<T>` |
| `INotifyPropertyChanged` | System.ComponentModel | Property change notifications | `PropertyChanged` event |
| `ICloneable` | System | Creates object copies | `Clone()` |
| `IFormattable` | System | Custom string formatting | `ToString(string, IFormatProvider)` |