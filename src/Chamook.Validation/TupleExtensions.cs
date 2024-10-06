namespace Chamook.Validation;

public static class TupleExtensions
{
    public static (A, B, C) Flatten<A, B, C>(this ((A, B), C) tuple) =>
        (tuple.Item1.Item1,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D) Flatten<A, B, C, D>(this (((A, B), C), D) tuple) =>
        (tuple.Item1.Item1.Item1,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E) Flatten<A, B, C, D, E>(this ((((A, B), C), D), E) tuple) =>
        (tuple.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E, F) Flatten<A, B, C, D, E, F>(this (((((A, B), C), D), E), F) tuple) =>
        (tuple.Item1.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E, F, G) Flatten<A, B, C, D, E, F, G>(this ((((((A, B), C), D), E), F), G) tuple) =>
        (tuple.Item1.Item1.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E, F, G, H) Flatten<A, B, C, D, E, F, G, H>(this (((((((A, B), C), D), E), F), G), H) tuple) =>
        (tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E, F, G, H, I) Flatten<A, B, C, D, E, F, G, H, I>(this ((((((((A, B), C), D), E), F), G), H), I) tuple) =>
        (tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);

    public static (A, B, C, D, E, F, G, H, I, J) Flatten<A, B, C, D, E, F, G, H, I, J>(this (((((((((A, B), C), D), E), F), G), H), I), J) tuple) =>
        (tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item1,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item1.Item2,
         tuple.Item1.Item1.Item2,
         tuple.Item1.Item2,
         tuple.Item2);
}
