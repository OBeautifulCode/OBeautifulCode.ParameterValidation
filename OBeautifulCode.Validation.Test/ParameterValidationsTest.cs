﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterValidationsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Validation.Recipes.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.AutoFakeItEasy;

    using Xunit;

    using static System.FormattableString;

    public static class ParameterValidationsTest
    {
        private static readonly ParameterEqualityComparer ParameterComparer = new ParameterEqualityComparer();

        private delegate Parameter Validation(Parameter parameter, string because = null);

        [Fact]
        public static void GetEnumerableGenericType___Gets_the_correct_generic_type___When_called_with_various_flavors_of_IEnumerable()
        {
            // Arrange
            var values1 = new[] { string.Empty };
            var values2 = new List<string> { string.Empty };
            var values3 = new ArrayList();
            var values4 = new Dictionary<string, object>();
            IEnumerable<string> values5 = new List<string>();
            IReadOnlyCollection<string> values6 = new List<string>();

            var expectedStringMessage = "validationName: BeOfNonExistentType, isElementInEnumerable: True, parameterValueTypeName: String";
            var expectedObjectMessage = "validationName: BeOfNonExistentType, isElementInEnumerable: True, parameterValueTypeName: Object";
            var expectedKvpMessage = "validationName: BeOfNonExistentType, isElementInEnumerable: True, parameterValueTypeName: KeyValuePair<string, object>";

            // Act
            // note: GetEnumerableGenericType is not public, so we're using BeOfNonExistentType which
            // always throws and checking that parameterValueTypeName is the expected type
            var actual1 = Record.Exception(() => values1.Must().Each().BeOfNonExistentType());
            var actual2 = Record.Exception(() => values2.Must().Each().BeOfNonExistentType());
            var actual3 = Record.Exception(() => values3.Must().Each().BeOfNonExistentType());
            var actual4 = Record.Exception(() => values4.Must().Each().BeOfNonExistentType());
            var actual5 = Record.Exception(() => values5.Must().Each().BeOfNonExistentType());
            var actual6 = Record.Exception(() => values6.Must().Each().BeOfNonExistentType());

            // Assert
            actual1.Should().BeOfType<InvalidCastException>();
            actual1.Message.Should().Be(expectedStringMessage);

            actual2.Should().BeOfType<InvalidCastException>();
            actual2.Message.Should().Be(expectedStringMessage);

            actual3.Should().BeOfType<InvalidCastException>();
            actual3.Message.Should().Be(expectedObjectMessage);

            actual4.Should().BeOfType<InvalidCastException>();
            actual4.Message.Should().Be(expectedKvpMessage);

            actual5.Should().BeOfType<InvalidCastException>();
            actual5.Message.Should().Be(expectedStringMessage);

            actual6.Should().BeOfType<InvalidCastException>();
            actual6.Message.Should().Be(expectedStringMessage);
        }

        [Fact]
        public static void BeNull___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.BeNull,
                ValidationName = nameof(ParameterValidation.BeNull),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is not null",
                InvalidCastExpectedTypes = "Any Reference Type, Nullable<T>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[] { null },
                MustEachPassingValues = new IEnumerable<Guid?>[] { new Guid?[] { null, null } },
                MustFailingValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty },
                MustEachFailingValues = new IEnumerable<Guid?>[] { new Guid?[] { null, Guid.NewGuid(), null } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[] { null },
                MustEachPassingValues = new IEnumerable<string>[] { new string[] { null, null } },
                MustFailingValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                MustEachFailingValues = new IEnumerable<string>[] { new string[] { null, string.Empty, null }, new string[] { null, " \r\n ", null }, new string[] { null, A.Dummy<string>(), null } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[] { null },
                MustEachPassingValues = new IEnumerable<object>[] { new object[] { null, null } },
                MustFailingValues = new object[] { A.Dummy<object>() },
                MustEachFailingValues = new IEnumerable<object>[] { new object[] { null, A.Dummy<object>(), null } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void NotBeNull___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.NotBeNull,
                ValidationName = nameof(ParameterValidation.NotBeNull),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is null",
                InvalidCastExpectedTypes = "Any Reference Type, Nullable<T>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Any Reference Type>, IEnumerable<Nullable<T>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustPassingValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty },
                MustEachPassingValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() } },
                MustFailingValues = new Guid?[] { null },
                MustEachFailingValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustPassingValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>() },
                MustEachPassingValues = new IEnumerable<string>[] { new string[] { string.Empty, " \r\n  ", A.Dummy<string>() } },
                MustFailingValues = new string[] { null },
                MustEachFailingValues = new IEnumerable<string>[] { new string[] { string.Empty, null, " \r\n  " } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustPassingValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachPassingValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() } },
                MustFailingValues = new object[] { null },
                MustEachFailingValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
        }

        [Fact]
        public static void BeTrue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.BeTrue,
                ValidationName = nameof(ParameterValidation.BeTrue),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is not true",
                InvalidCastExpectedTypes = "Boolean, Nullable<Boolean>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Boolean>, IEnumerable<Nullable<Boolean>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustInvalidTypeValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty, null },
                MustEachInvalidTypeValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() }, new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustInvalidTypeValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>(), null },
                MustEachInvalidTypeValues = new IEnumerable<string>[] { new string[] { string.Empty, " \r\n  ", A.Dummy<string>() }, new string[] { string.Empty, null, " \r\n  " } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustInvalidTypeValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachInvalidTypeValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[] { true },
                MustEachPassingValues = new[] { new[] { true, true } },
                MustFailingValues = new[] { false },
                MustEachFailingValues = new[] { new[] { false, false } },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[] { true },
                MustEachPassingValues = new[] { new bool?[] { true, true } },
                MustFailingValues = new bool?[] { false, null },
                MustEachFailingValues = new[] { new bool?[] { false, null }, new bool?[] { null, false } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeTrue___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.NotBeTrue,
                ValidationName = nameof(ParameterValidation.NotBeTrue),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is true",
                InvalidCastExpectedTypes = "Boolean, Nullable<Boolean>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Boolean>, IEnumerable<Nullable<Boolean>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustInvalidTypeValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty, null },
                MustEachInvalidTypeValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() }, new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustInvalidTypeValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>(), null },
                MustEachInvalidTypeValues = new IEnumerable<string>[] { new string[] { string.Empty, " \r\n  ", A.Dummy<string>() }, new string[] { string.Empty, null, " \r\n  " } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustInvalidTypeValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachInvalidTypeValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[] { false },
                MustEachPassingValues = new[] { new[] { false, false } },
                MustFailingValues = new[] { true },
                MustEachFailingValues = new[] { new[] { true, true } },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[] { false, null },
                MustEachPassingValues = new[] { new bool?[] { false, null }, new bool?[] { null, false } },
                MustFailingValues = new bool?[] { true },
                MustEachFailingValues = new[] { new bool?[] { true, true } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void BeFalse___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.BeFalse,
                ValidationName = nameof(ParameterValidation.BeFalse),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is not false",
                InvalidCastExpectedTypes = "Boolean, Nullable<Boolean>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Boolean>, IEnumerable<Nullable<Boolean>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustInvalidTypeValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty, null },
                MustEachInvalidTypeValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() }, new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustInvalidTypeValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>(), null },
                MustEachInvalidTypeValues = new IEnumerable<string>[] { new string[] { string.Empty, " \r\n  ", A.Dummy<string>() }, new string[] { string.Empty, null, " \r\n  " } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustInvalidTypeValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachInvalidTypeValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[] { false },
                MustEachPassingValues = new[] { new[] { false, false } },
                MustFailingValues = new[] { true },
                MustEachFailingValues = new[] { new[] { true, true } },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[] { false },
                MustEachPassingValues = new[] { new bool?[] { false } },
                MustFailingValues = new bool?[] { true, null },
                MustEachFailingValues = new[] { new bool?[] { true, null }, new bool?[] { null, true } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeFalse___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest = new ValidationTest
            {
                Validation = ParameterValidation.NotBeFalse,
                ValidationName = nameof(ParameterValidation.NotBeFalse),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is false",
                InvalidCastExpectedTypes = "Boolean, Nullable<Boolean>",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<Boolean>, IEnumerable<Nullable<Boolean>>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustInvalidTypeValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty, null },
                MustEachInvalidTypeValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() }, new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var stringTestValues = new TestValues<string>
            {
                MustInvalidTypeValues = new string[] { string.Empty, " \r\n  ", A.Dummy<string>(), null },
                MustEachInvalidTypeValues = new IEnumerable<string>[] { new string[] { string.Empty, " \r\n  ", A.Dummy<string>() }, new string[] { string.Empty, null, " \r\n  " } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustInvalidTypeValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachInvalidTypeValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            var boolTestValues = new TestValues<bool>
            {
                MustPassingValues = new[] { true },
                MustEachPassingValues = new[] { new[] { true, true } },
                MustFailingValues = new[] { false },
                MustEachFailingValues = new[] { new[] { false, false } },
            };

            var nullableBoolTestValues = new TestValues<bool?>
            {
                MustPassingValues = new bool?[] { true, null },
                MustEachPassingValues = new[] { new bool?[] { true, null }, new bool?[] { null, true } },
                MustFailingValues = new bool?[] { false },
                MustEachFailingValues = new[] { new bool?[] { false, false } },
            };

            // Act, Assert
            validationTest.Run(guidTestValues);
            validationTest.Run(nullableGuidTestValues);
            validationTest.Run(stringTestValues);
            validationTest.Run(objectTestValues);
            validationTest.Run(boolTestValues);
            validationTest.Run(nullableBoolTestValues);
        }

        [Fact]
        public static void NotBeNullNorWhiteSpace___Should_throw_or_not_throw_as_expected___When_called()
        {
            // Arrange
            var validationTest1 = new ValidationTest
            {
                Validation = ParameterValidation.NotBeNullNorWhiteSpace,
                ValidationName = nameof(ParameterValidation.NotBeNullNorWhiteSpace),
                ExceptionType = typeof(ArgumentNullException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is null",
                InvalidCastExpectedTypes = "String",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<String>",
            };

            var validationTest2 = new ValidationTest
            {
                Validation = ParameterValidation.NotBeNullNorWhiteSpace,
                ValidationName = nameof(ParameterValidation.NotBeNullNorWhiteSpace),
                ExceptionType = typeof(ArgumentException),
                EachExceptionType = typeof(ArgumentException),
                ExceptionMessageSuffix = "is white space",
                InvalidCastExpectedTypes = "String",
                InvalidCastExpectedEnumerableTypes = "IEnumerable<String>",
            };

            var guidTestValues = new TestValues<Guid>
            {
                MustInvalidTypeValues = new[] { Guid.Empty, Guid.NewGuid() },
                MustEachInvalidTypeValues = new IEnumerable<Guid>[] { new Guid[] { }, new Guid[] { Guid.NewGuid() } },
            };

            var nullableGuidTestValues = new TestValues<Guid?>
            {
                MustInvalidTypeValues = new Guid?[] { A.Dummy<Guid>(), Guid.Empty, null },
                MustEachInvalidTypeValues = new IEnumerable<Guid?>[] { new Guid?[] { Guid.NewGuid(), Guid.NewGuid() }, new Guid?[] { Guid.NewGuid(), null, Guid.NewGuid() } },
            };

            var objectTestValues = new TestValues<object>
            {
                MustInvalidTypeValues = new object[] { A.Dummy<object>(), new List<string>() { null } },
                MustEachInvalidTypeValues = new IEnumerable<object>[] { new object[] { A.Dummy<object>(), A.Dummy<object>() }, new object[] { A.Dummy<object>(), null, A.Dummy<object>() } },
            };

            var stringTestValues1 = new TestValues<string>
            {
                MustPassingValues = new string[] { A.Dummy<string>() },
                MustEachPassingValues = new[] { new string[] { A.Dummy<string>() } },
                MustFailingValues = new string[] { null },
                MustEachFailingValues = new[] { new string[] { A.Dummy<string>(), null, A.Dummy<string>() } },
            };

            var stringTestValues2 = new TestValues<string>
            {
                MustPassingValues = new string[] { A.Dummy<string>() },
                MustEachPassingValues = new[] { new string[] { A.Dummy<string>() } },
                MustFailingValues = new string[] { string.Empty, "    ", " \r\n  " },
                MustEachFailingValues = new[] { new string[] { A.Dummy<string>(), string.Empty, A.Dummy<string>() }, new string[] { A.Dummy<string>(), "    ", A.Dummy<string>() }, new string[] { A.Dummy<string>(), " \r\n  ", A.Dummy<string>() } },
            };

            // Act, Assert
            validationTest1.Run(guidTestValues);
            validationTest1.Run(nullableGuidTestValues);
            validationTest1.Run(objectTestValues);
            validationTest1.Run(stringTestValues1);

            validationTest2.Run(guidTestValues);
            validationTest2.Run(nullableGuidTestValues);
            validationTest2.Run(objectTestValues);
            validationTest2.Run(stringTestValues2);
        }

        private static void Run<T>(
            this ValidationTest validationTest,
            TestValues<T> testValues)
        {
            var parameterNames = new[] { null, A.Dummy<string>() };
            var becauses = new[] { null, A.Dummy<string>() };

            foreach (var parameterName in parameterNames)
            {
                foreach (var because in becauses)
                {
                    RunPassingScenarios(validationTest, testValues, parameterName, because);

                    RunMustFailingScenarios(validationTest, testValues, parameterName, because);

                    RunMustEachImproperUseOfFrameworkScenarios<T>(validationTest, parameterName, because);

                    RunMustEachFailingScenarios(validationTest, testValues, parameterName, because);

                    RunMustInvalidParameterTypeScenarios(validationTest, testValues, parameterName, because);

                    RunMustEachInvalidParameterTypeScenarios(validationTest, testValues, parameterName, because);
                }
            }
        }

        private static void RunPassingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            string because)
        {
            var mustParameters = testValues.MustPassingValues.Select(_ => _.Named(parameterName).Must());
            var mustEachParameters = testValues.MustEachPassingValues.Select(_ => _.Named(parameterName).Must().Each());
            var parameters = mustParameters.Concat(mustEachParameters).ToList();

            foreach (var parameter in parameters)
            {
                // Arrange
                var expected = parameter.CloneWithHasBeenValidated();

                // Act
                var actual = validationTest.Validation(parameter, because);

                // Assert
                ParameterComparer.Equals(actual, expected).Should().BeTrue();
            }
        }

        private static void RunMustFailingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            string because)
        {
            foreach (var failingValue in testValues.MustFailingValues)
            {
                // Arrange
                var parameter = failingValue.Named(parameterName).Must();
                var expectedExceptionMessage = because;
                if (expectedExceptionMessage == null)
                {
                    if (parameterName == null)
                    {
                        expectedExceptionMessage = "parameter " + validationTest.ExceptionMessageSuffix;
                    }
                    else
                    {
                        expectedExceptionMessage = "parameter '" + parameterName + "' " + validationTest.ExceptionMessageSuffix;
                    }
                }

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, because));

                // Assert
                actual.Should().BeOfType(validationTest.ExceptionType);
                actual.Message.Should().Be(expectedExceptionMessage);
            }
        }

        private static void RunMustEachFailingScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            string because)
        {
            foreach (var eachFailingValue in testValues.MustEachFailingValues)
            {
                // Arrange
                var parameter = eachFailingValue.Named(parameterName).Must().Each();
                var expectedExceptionMessage = because;
                if (expectedExceptionMessage == null)
                {
                    if (parameterName == null)
                    {
                        expectedExceptionMessage = "parameter contains an element that " + validationTest.ExceptionMessageSuffix;
                    }
                    else
                    {
                        expectedExceptionMessage = "parameter '" + parameterName + "' contains an element that " + validationTest.ExceptionMessageSuffix;
                    }
                }

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, because));

                // Assert
                actual.Should().BeOfType(validationTest.EachExceptionType);
                actual.Message.Should().Be(expectedExceptionMessage);
            }
        }

        private static void RunMustInvalidParameterTypeScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            string because)
        {
            foreach (var invalidTypeValue in testValues.MustInvalidTypeValues)
            {
                // Arrange
                var parameter = invalidTypeValue.Named(parameterName).Must();
                var expectedMessage = Invariant($"called {validationTest.ValidationName}() on an object that is not one of the following types: {validationTest.InvalidCastExpectedTypes}");

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, because));

                // Assert
                actual.Should().BeOfType<InvalidCastException>();
                actual.Message.Should().Be(expectedMessage);
            }
        }

        private static void RunMustEachInvalidParameterTypeScenarios<T>(
            ValidationTest validationTest,
            TestValues<T> testValues,
            string parameterName,
            string because)
        {
            foreach (var invalidTypeValue in testValues.MustEachInvalidTypeValues)
            {
                // Arrange
                var parameter = invalidTypeValue.Named(parameterName).Must().Each();
                var expectedMessage = Invariant($"called {validationTest.ValidationName}() on an object that is not one of the following types: {validationTest.InvalidCastExpectedEnumerableTypes}");

                // Act
                var actual = Record.Exception(() => validationTest.Validation(parameter, because));

                // Assert
                actual.Should().BeOfType<InvalidCastException>();
                actual.Message.Should().Be(expectedMessage);
            }
        }

        private static void RunMustEachImproperUseOfFrameworkScenarios<T>(
            ValidationTest validationTest,
            string parameterName,
            string because)
        {
            // Arrange
            // calling Each() on IEnumerable that is null OR a value that's not IEnumerable
            IEnumerable<string> nullEnumerable = null;
            var parameter1 = nullEnumerable.Named(parameterName).Must();
            parameter1.HasBeenEached = true;

            object notEnumerable = new object();
            var parameter2 = notEnumerable.Named(parameterName).Must();
            parameter2.HasBeenEached = true;

            // Act
            var actual1 = Record.Exception(() => validationTest.Validation(parameter1, because));
            var actual2 = Record.Exception(() => validationTest.Validation(parameter2, because));

            // Assert
            actual1.Should().BeOfType<InvalidOperationException>();
            actual1.Message.Should().Be(ParameterValidator.ImproperUseOfFrameworkExceptionMessage);

            actual2.Should().BeOfType<InvalidOperationException>();
            actual2.Message.Should().Be(ParameterValidator.ImproperUseOfFrameworkExceptionMessage);
        }

        private class ValidationTest
        {
            public Validation Validation { get; set; }

            public Type ExceptionType { get; set; }

            public Type EachExceptionType { get; set; }

            public string ExceptionMessageSuffix { get; set; }

            public string InvalidCastExpectedTypes { get; set; }

            public string InvalidCastExpectedEnumerableTypes { get; set; }

            public string ValidationName { get; set; }
        }

        private class TestValues<T>
        {
            public IReadOnlyCollection<T> MustInvalidTypeValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachInvalidTypeValues { get; set; } = new List<List<T>>();

            public IReadOnlyCollection<T> MustPassingValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachPassingValues { get; set; } = new List<List<T>>();

            public IReadOnlyCollection<T> MustFailingValues { get; set; } = new List<T>();

            public IReadOnlyCollection<IEnumerable<T>> MustEachFailingValues { get; set; } = new List<List<T>>();
        }
    }
}