﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.TypeValidation.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Validation source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Validation.Recipes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using static System.FormattableString;

    /// <summary>
    /// Contains all validations that can be applied to a <see cref="Parameter"/>.
    /// </summary>
#if !OBeautifulCodeValidationRecipesProject
    [System.Diagnostics.DebuggerStepThrough]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Validation", "See package version number")]
    internal
#else
    public
#endif
        static partial class ParameterValidation
    {
        private delegate void TypeValidationHandler(string validationName, bool isElementInEnumerable, Type valueType, Type[] referenceTypes, ValidationParameter[] validationParameters);

        private static readonly Type EnumerableType = typeof(IEnumerable);

        private static readonly Type UnboundGenericEnumerableType = typeof(IEnumerable<>);

        private static readonly Type ComparableType = typeof(IComparable);

        private static readonly Type UnboundGenericComparableType = typeof(IComparable<>);

        private static readonly Type ObjectType = typeof(object);

        private static readonly IReadOnlyCollection<TypeValidation> MustBeNullableTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfTypeCannotBeNull,
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> MustBeBooleanTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(bool), typeof(bool?) },
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> MustBeStringTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(string) },
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> MustBeGuidTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(Guid), typeof(Guid?) },
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> MustBeEnumerableTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(IEnumerable) },
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> MustBeEnumerableOfNullableTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(IEnumerable) },
            },
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfEnumerableTypeCannotBeNull,
            }
        };

        private static readonly IReadOnlyCollection<TypeValidation> InequalityTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotComparable,
            },
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
            },
        };

        private static readonly IReadOnlyCollection<TypeValidation> EqualsTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualValueType,
            },
        };

        private static readonly IReadOnlyCollection<TypeValidation> AlwaysThrowTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = Throw,
            },
        };

        private static readonly IReadOnlyCollection<TypeValidation> ContainmentTypeValidations = new[]
        {
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfNotOfType,
                ReferenceTypes = new[] { typeof(IEnumerable) },
            },
            new TypeValidation
            {
                TypeValidationHandler = ThrowIfAnyValidationParameterTypeDoesNotEqualEnumerableValueType,
            },
        };

        // ReSharper disable once UnusedParameter.Local
        private static void Throw(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] referenceTypes,
            ValidationParameter[] validationParameters)
        {
            var parameterValueTypeName = valueType.GetFriendlyTypeName();
            throw new InvalidCastException(Invariant($"validationName: {validationName}, isElementInEnumerable: {isElementInEnumerable}, parameterValueTypeName: {parameterValueTypeName}"));
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfTypeCannotBeNull(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] referenceTypes,
            ValidationParameter[] validationParameters)
        {
            if (valueType.IsValueType && (Nullable.GetUnderlyingType(valueType) == null))
            {
                ThrowOnParameterUnexpectedType(validationName, isElementInEnumerable, AnyReferenceTypeName, NullableGenericTypeName);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfEnumerableTypeCannotBeNull(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] referenceTypes,
            ValidationParameter[] validationParameters)
        {
            var enumerableType = GetEnumerableGenericType(valueType);

            if (enumerableType.IsValueType && (Nullable.GetUnderlyingType(enumerableType) == null))
            {
                ThrowOnParameterUnexpectedType(validationName, isElementInEnumerable, nameof(IEnumerable), EnumerableOfAnyReferenceTypeName, EnumerableOfNullableGenericTypeName);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfNotOfType(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] validTypes,
            ValidationParameter[] validationParameters)
        {
            if ((!validTypes.Contains(valueType)) && (!validTypes.Any(_ => _.IsAssignableFrom(valueType))))
            {
                ThrowOnParameterUnexpectedType(validationName, isElementInEnumerable, validTypes);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfNotComparable(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] referenceTypes,
            ValidationParameter[] validationParameters)
        {
            // type is IComparable or can be assigned to IComparable
            if ((valueType != ComparableType) && (!ComparableType.IsAssignableFrom(valueType)))
            {
                // type is IComparable<T>
                if ((!valueType.IsGenericType) || (valueType.GetGenericTypeDefinition() != UnboundGenericComparableType))
                {
                    // type implements IComparable<T>
                    var comparableType = valueType.GetInterfaces().FirstOrDefault(_ => _.IsGenericType && (_.GetGenericTypeDefinition() == UnboundGenericEnumerableType));
                    if (comparableType == null)
                    {
                        // note that, for completeness, we should recurse through all interface implementations
                        // and check whether any of those are IComparable<>
                        // see: https://stackoverflow.com/questions/5461295/using-isassignablefrom-with-open-generic-types
                        ThrowOnParameterUnexpectedType(validationName, isElementInEnumerable, nameof(IComparable), ComparableGenericTypeName);
                    }
                }
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfAnyValidationParameterTypeDoesNotEqualValueType(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] validTypes,
            ValidationParameter[] validationParameters)
        {
            foreach (var validationParameter in validationParameters)
            {
                if (validationParameter.ValueType != valueType)
                {
                    ThrowOnValidationParameterUnexpectedType(validationName, validationParameter.Name, valueType);
                }
            }
        }

        private static void ThrowIfAnyValidationParameterTypeDoesNotEqualEnumerableValueType(
            string validationName,
            bool isElementInEnumerable,
            Type valueType,
            Type[] validTypes,
            ValidationParameter[] validationParameters)
        {
            var enumerableType = GetEnumerableGenericType(valueType);

            foreach (var validationParameter in validationParameters)
            {
                if (validationParameter.ValueType != enumerableType)
                {
                    ThrowOnValidationParameterUnexpectedType(validationName, validationParameter.Name, enumerableType);
                }
            }
        }

        private class TypeValidation
        {
            public TypeValidationHandler TypeValidationHandler { get; set; }

            public Type[] ReferenceTypes { get; set; }
        }
    }
}
