﻿using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions;

public class ExpressionEvaluatorTest
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory = Substitute.For<IExpressionStrategyFactory>();

    [Fact(DisplayName = "applies assignment interpreting strategy on assignment expressions")]
    public void applies_assignment_interpreting_strategy_on_assignment_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory);
        var assignmentExpression = Substitute.For<AssignmentExpression>();
        var strategy = Substitute.For<ExpressionInterpretingStrategy>();
        expressionStrategyFactory.GetAssignmentInterpretingStrategy(assignmentExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(assignmentExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies binary expression interpreting strategy on binary expressions")]
    public void applies_binary_expression_interpreting_strategy_on_binary_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory);
        var binaryExpression = Substitute.For<BinaryExpression>();
        var strategy = Substitute.For<ExpressionInterpretingStrategy>();
        expressionStrategyFactory.GetBinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(binaryExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies fixes expressions interpreting strategy on prefix and postfix expressions")]
    public void applies_fixes_expressions_interpreting_strategy_on_prefix_and_postfix_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory);
        var fixExpression = Substitute.For<FixExpression>();
        var strategy = Substitute.For<ExpressionInterpretingStrategy>();
        expressionStrategyFactory.GetFixesInterpretingStrategy(fixExpression).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(fixExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies unary expression interpreting strategy on unary expressions")]
    public void applies_unary_expression_interpreting_strategy_on_unary_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory);
        var unaryExpression = Substitute.For<UnaryExpression>();
        var strategy = Substitute.For<ExpressionInterpretingStrategy>();
        expressionStrategyFactory.GetUnaryExpressionInterpretingStrategy(unaryExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(unaryExpression);
        Assert.Equal(expectedResult, result);
    }
}