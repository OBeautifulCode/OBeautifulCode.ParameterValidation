﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.cs" company="OBeautifulCode">
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
        static class ParameterValidation
    {
        private delegate void TypeValidation(string validationName, bool isElementInEnumerable, Type parameterValueType);

        private delegate void ValueValidation(object parameterValue, string parameterName, string because, bool isElementInEnumerable);

        /// <summary>
        /// Validates that the parameter is null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeNull(
            this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfCannotBeNull,
            };

            parameter.Validate(BeNull, nameof(BeNull), typeValidations, because);
            return parameter;
        }

        /// <summary>
        /// Validates that the parameter is not null.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter NotBeNull(
            this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                ThrowIfCannotBeNull,
            };

            parameter.Validate(NotBeNull, nameof(NotBeNull), typeValidations, because);
            return parameter;
        }

        /// <summary>
        /// Validates that the parameter is true.
        /// </summary>
        /// <param name="parameter">The parameter to validate.</param>
        /// <param name="because">Rationale for the validation.  Replaces the default exception message constructed by this validation.</param>
        /// <returns>
        /// The validated parameter.
        /// </returns>
        public static Parameter BeTrue(
            this Parameter parameter,
            string because = null)
        {
            var typeValidations = new TypeValidation[]
            {
                (validationName, isElementInEnumerable, parameterValueType) => ThrowIfNotOfType(validationName, isElementInEnumerable, parameterValueType, typeof(bool), typeof(bool?)),
            };

            parameter.Validate(BeTrue, nameof(BeTrue), typeValidations, because);
            return parameter;
        }

        private static void Validate(
            this Parameter parameter,
            ValueValidation valueValidation,
            string validationName,
            IReadOnlyCollection<TypeValidation> typeValidations,
            string because)
        {
            ParameterValidator.ThrowOnImproperUseOfFramework(parameter, ParameterShould.BeMusted);

            if (parameter.HasBeenEached)
            {
                if (parameter.Value is IEnumerable valueAsEnumerable)
                {
                    var enumerableType = GetEnumerableGenericType(parameter.ValueType);

                    foreach (var typeValidation in typeValidations)
                    {
                        typeValidation(validationName, isElementInEnumerable: true, parameterValueType: enumerableType);
                    }

                    foreach (var element in valueAsEnumerable)
                    {
                        valueValidation(element, parameter.Name, because, isElementInEnumerable: true);
                    }
                }
                else
                {
                    // Each() calls:
                    // - ThrowOnImproperUseOfFramework when the parameter value is null
                    // - ThrowOnUnexpectedType when the parameter value is not an Enumerable
                    // so if we get here, the caller is trying to hack the framework
                    ParameterValidator.ThrowOnImproperUseOfFramework();
                }
            }
            else
            {
                foreach (var typeValidation in typeValidations)
                {
                    typeValidation(validationName, isElementInEnumerable: false, parameterValueType: parameter.ValueType);
                }

                valueValidation(parameter.Value, parameter.Name, because, isElementInEnumerable: false);
            }

            parameter.HasBeenValidated = true;
        }

        private static Type GetEnumerableGenericType(
            Type type)
        {
            // adapted from: https://stackoverflow.com/a/17713382/356790

            Type result;
            if (type.IsArray)
            {
                // type is array, shortcut
                result = type.GetElementType();
            }
            else if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                // type is IEnumerable<T>
                result = type.GetGenericArguments()[0];
            }
            else
            {
                // type implements/extends IEnumerable<T>
                result = type
                    .GetInterfaces()
                    .Where(_ => _.IsGenericType && _.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(_ => _.GenericTypeArguments[0])
                    .FirstOrDefault();

                if (result == null)
                {
                    result = typeof(object);
                }
            }

            return result;
        }

        private static void ThrowIfCannotBeNull(
            string validationName,
            bool isElementInEnumerable,
            Type parameterValueType)
        {
            if (parameterValueType.IsValueType && (Nullable.GetUnderlyingType(parameterValueType) == null))
            {
                ParameterValidator.ThrowOnUnexpectedTypes(validationName, isElementInEnumerable, "Any Reference Type", "Nullable<T>");
            }            
        }

        private static void ThrowIfNotOfType(
            string validationName,
            bool isElementInEnumerable,
            Type parameterValueType,
            params Type[] validTypes)
        {
            if (!validTypes.Contains(parameterValueType))
            {
                ParameterValidator.ThrowOnUnexpectedTypes(validationName, isElementInEnumerable, validTypes);
            }
        }

        private static string BuildExceptionMessage(
            string parameterName,
            string because,
            bool isElementInEnumerable,
            string exceptionMessageSuffix)
        {
            if (because != null)
            {
                return because;
            }

            var parameterNameQualifier = parameterName == null ? string.Empty : Invariant($" '{parameterName}'");
            var enumerableQualifier = isElementInEnumerable ? " contains an element that" : string.Empty;
            var result = Invariant($"parameter{parameterNameQualifier}{enumerableQualifier} {exceptionMessageSuffix}");
            return result;
        }

        private static void BeNull(
            object parameterValue,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (!ReferenceEquals(parameterValue, null))
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not null");
                throw new ArgumentException(exceptionMessage);
            }
        }

        private static void NotBeNull(
            object parameterValue,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            if (ReferenceEquals(parameterValue, null))
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is null");
                if (isElementInEnumerable)
                {
                    throw new ArgumentException(exceptionMessage);
                }
                else
                {
                    throw new ArgumentNullException(null, exceptionMessage);
                }
            }
        }

        private static void BeTrue(
            object parameterValue,
            string parameterName,
            string because,
            bool isElementInEnumerable)
        {
            var shouldThrow = ReferenceEquals(parameterValue, null) || ((bool)parameterValue != true);
            if (shouldThrow)
            {
                var exceptionMessage = BuildExceptionMessage(parameterName, because, isElementInEnumerable, "is not true");
                throw new ArgumentException(exceptionMessage);
            }
        }
    }
}
