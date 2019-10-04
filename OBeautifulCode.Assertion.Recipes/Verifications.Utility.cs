﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidation.Utility.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Assertion.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Contains all validations that can be applied to a <see cref="AssertionTracker"/>.
    /// </summary>
#if !OBeautifulCodeAssertionRecipesProject
    internal
#else
    public
#endif
        static partial class Verifications
    {
#pragma warning disable SA1201

        private static readonly MethodInfo GetDefaultValueOpenGenericMethodInfo = ((Func<object>)GetDefaultValue<object>).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> GetDefaultValueTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly MethodInfo EqualsUsingDefaultEqualityComparerOpenGenericMethodInfo = ((Func<object, object, bool>)EqualsUsingDefaultEqualityComparer).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static readonly MethodInfo CompareUsingDefaultComparerOpenGenericMethodInfo = ((Func<object, object, CompareOutcome>)CompareUsingDefaultComparer).Method.GetGenericMethodDefinition();

        private static readonly ConcurrentDictionary<Type, MethodInfo> CompareUsingDefaultComparerTypeToMethodInfoMap = new ConcurrentDictionary<Type, MethodInfo>();

        private static void ExecuteVerification(
            this AssertionTracker assertionTracker,
            Verification verification)
        {
            WorkflowExtensions.ThrowImproperUseOfFrameworkIfDetected(assertionTracker, AssertionTrackerShould.BeMusted);

            var hasBeenEached = assertionTracker.Actions.HasFlag(Actions.Eached);

            verification.SubjectName = assertionTracker.SubjectName;
            verification.IsElementInEnumerable = hasBeenEached;

            if (hasBeenEached)
            {
                // check that the parameter is an IEnumerable and not null
                if (!assertionTracker.Actions.HasFlag(Actions.VerifiedAtLeastOnce))
                {
                    var eachValidation = new Verification
                    {
                        SubjectName = assertionTracker.SubjectName,
                        Name = nameof(WorkflowExtensions.Each),
                        Value = assertionTracker.SubjectValue,
                        ValueType = assertionTracker.SubjectType,
                        IsElementInEnumerable = false,
                    };

                    ThrowIfNotOfType(eachValidation, MustBeEnumerableTypeValidations.Single());

                    NotBeNullInternal(eachValidation);
                }

                var valueAsEnumerable = (IEnumerable)assertionTracker.SubjectValue;
                var enumerableType = assertionTracker.SubjectType.GetEnumerableElementType();
                verification.ValueType = enumerableType;

                foreach (var typeValidation in verification.TypeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.Handler(verification, typeValidation);
                }

                foreach (var element in valueAsEnumerable)
                {
                    verification.Value = element;

                    verification.Handler(verification);
                }
            }
            else
            {
                verification.Value = assertionTracker.SubjectValue;
                verification.ValueType = assertionTracker.SubjectType;

                foreach (var typeValidation in verification.TypeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.Handler(verification, typeValidation);
                }

                verification.Handler(verification);
            }

            assertionTracker.Actions |= Actions.VerifiedAtLeastOnce;
        }

        private static void ThrowIfMalformedRange(
            VerificationParameter[] validationParameters)
        {
            // the public BeInRange/NotBeInRange is generic and guarantees that minimum and maximum are of the same type
            var rangeIsMalformed = CompareUsingDefaultComparer(validationParameters[0].Type, validationParameters[0].Value, validationParameters[1].Value) == CompareOutcome.Value1GreaterThanValue2;
            if (rangeIsMalformed)
            {
                var malformedRangeExceptionMessage = string.Format(CultureInfo.InvariantCulture, MalformedRangeExceptionMessage, validationParameters[0].Name, validationParameters[1].Name, validationParameters[0].Value?.ToString() ?? NullValueToString, validationParameters[1].Value?.ToString() ?? NullValueToString);
                WorkflowExtensions.ThrowImproperUseOfFramework(malformedRangeExceptionMessage);
            }
        }

        private static string BuildArgumentExceptionMessage(
            Verification verification,
            string exceptionMessageSuffix,
            Include include = Include.None,
            Type genericTypeOverride = null)
        {
            if (verification.ApplyBecause == ApplyBecause.InLieuOfDefaultMessage)
            {
                // we force to empty string if null because otherwise when the exception is
                // constructed the framework chooses some generic message like 'An exception of type ArgumentException was thrown'
                return verification.Because ?? string.Empty;
            }

            var parameterNameQualifier = verification.SubjectName == null ? string.Empty : Invariant($" '{verification.SubjectName}'");
            var enumerableQualifier = verification.IsElementInEnumerable ? " contains an element that" : string.Empty;
            var genericTypeQualifier = include.HasFlag(Include.GenericType) ? ", where T: " + (genericTypeOverride?.ToStringReadable() ?? verification.ValueType.ToStringReadable()) : string.Empty;
            var failingValueQualifier = include.HasFlag(Include.FailingValue) ? (verification.IsElementInEnumerable ? "  Element value" : "  Parameter value") + Invariant($" is '{verification.Value?.ToString() ?? NullValueToString}'.") : string.Empty;
            var validationParameterQualifiers = verification.VerificationParameters == null || !verification.VerificationParameters.Any() ? string.Empty : string.Join(string.Empty, verification.VerificationParameters.Select(_ => _.ToExceptionMessageComponent()));
            var result = Invariant($"Parameter{parameterNameQualifier}{enumerableQualifier} {exceptionMessageSuffix}{genericTypeQualifier}.{failingValueQualifier}{validationParameterQualifiers}");

            if (verification.ApplyBecause == ApplyBecause.PrefixedToDefaultMessage)
            {
                if (!string.IsNullOrWhiteSpace(verification.Because))
                {
                    result = verification.Because + "  " + result;
                }
            }
            else if (verification.ApplyBecause == ApplyBecause.SuffixedToDefaultMessage)
            {
                if (!string.IsNullOrWhiteSpace(verification.Because))
                {
                    result = result + "  " + verification.Because;
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(ApplyBecause)} is not supported: {verification.ApplyBecause}"));
            }

            return result;
        }

        private static string ToExceptionMessageComponent(
            this VerificationParameter verificationParameter)
        {
            var result = Invariant($"  Specified '{verificationParameter.Name}' is");
            if (verificationParameter.ValueToStringFunc == null)
            {
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                if (verificationParameter.Value == null)
                {
                    result = Invariant($"{result} '{NullValueToString}'");
                }
                else
                {
                    result = Invariant($"{result} '{verificationParameter.Value}'");
                }
            }
            else
            {
                result = Invariant($"{result} {verificationParameter.ValueToStringFunc()}");
            }

            result = Invariant($"{result}.");

            return result;
        }

        private static Exception AddData(
            this Exception exception,
            IDictionary data)
        {
            if (data != null)
            {
                // because the caller is creating a new exception, we know that Data is empty
                // and we don't have to check for key conflicts (same key exists in both exception.Data and in data)
                foreach (var dataKey in data.Keys)
                {
                    exception.Data[dataKey] = data[dataKey];
                }
            }

            return exception;
        }

        private static T GetDefaultValue<T>()
        {
            var result = default(T);
            return result;
        }

        private static object GetDefaultValue(
            Type type)
        {
            if (!GetDefaultValueTypeToMethodInfoMap.ContainsKey(type))
            {
                GetDefaultValueTypeToMethodInfoMap.TryAdd(type, GetDefaultValueOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            var result = GetDefaultValueTypeToMethodInfoMap[type].Invoke(null, null);
            return result;
        }

        private static bool EqualsUsingDefaultEqualityComparer<T>(
            T value1,
            T value2)
        {
            var result = EqualityComparer<T>.Default.Equals(value1, value2);
            return result;
        }

        private static bool EqualUsingDefaultEqualityComparer(
            Type type,
            object value1,
            object value2)
        {
            if (!EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap.ContainsKey(type))
            {
                EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap.TryAdd(type, EqualsUsingDefaultEqualityComparerOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            var result = (bool)EqualsUsingDefaultEqualityComparerTypeToMethodInfoMap[type].Invoke(null, new[] { value1, value2 });
            return result;
        }

        private static CompareOutcome CompareUsingDefaultComparer<T>(
            T x,
            T y)
        {
            var comparison = Comparer<T>.Default.Compare(x, y);
            CompareOutcome result;
            if (comparison < 0)
            {
                result = CompareOutcome.Value1LessThanValue2;
            }
            else if (comparison == 0)
            {
                result = CompareOutcome.Value1EqualsValue2;
            }
            else
            {
                result = CompareOutcome.Value1GreaterThanValue2;
            }

            return result;
        }

        private static CompareOutcome CompareUsingDefaultComparer(
            Type type,
            object value1,
            object value2)
        {
            if (!CompareUsingDefaultComparerTypeToMethodInfoMap.ContainsKey(type))
            {
                CompareUsingDefaultComparerTypeToMethodInfoMap.TryAdd(type, CompareUsingDefaultComparerOpenGenericMethodInfo.MakeGenericMethod(type));
            }

            // note that the call is ultimately, via reflection, to Compare(T, T)
            // as such, reflection will throw an ArgumentException if the types of value1 and value2 are
            // not "convertible" to the specified type.  It's a pretty complicated heuristic:
            // https://stackoverflow.com/questions/34433043/check-whether-propertyinfo-setvalue-will-throw-an-argumentexception
            // Instead of relying on this heuristic, we just check upfront that value2's type == the specified type
            // (value1's type will always be the specified type).  This constrains our capabilities - for example, we
            // can't compare an integer to a decimal.  That said, we feel like this is a good constraint in a parameter
            // validation framework.  We'd rather be forced to make the types align than get a false negative
            // (a validation passes when it should fail).

            // otherwise, if reflection is able to call Compare(T, T), then ArgumentException can be thrown if
            // Type T does not implement either the System.IComparable<T> generic interface or the System.IComparable interface
            // However we already check for this upfront in ThrowIfNotComparable
            var result = (CompareOutcome)CompareUsingDefaultComparerTypeToMethodInfoMap[type].Invoke(null, new[] { value1, value2 });

            return result;
        }

        private enum CompareOutcome
        {
            Value1LessThanValue2,

            Value1EqualsValue2,

            Value1GreaterThanValue2,
        }

        [Flags]
        private enum Include
        {
            None = 0,

            FailingValue = 1,

            GenericType = 2,
        }
#pragma warning restore SA1201
    }
}
