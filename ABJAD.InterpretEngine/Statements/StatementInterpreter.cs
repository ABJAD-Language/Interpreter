﻿using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;

namespace ABJAD.InterpretEngine.Statements;

public class StatementInterpreter : Interpreter<Statement>
{
    private readonly ScopeFacade scope;
    private readonly Evaluator<Expression> expressionEvaluator;
    private readonly DeclarationInterpreter declarationInterpreter;

    public StatementInterpreter(ScopeFacade scope)
    {
        this.scope = scope;
        expressionEvaluator = new ExpressionEvaluator(scope);
        declarationInterpreter = new DeclarationInterpreter(scope);
    }

    public void Interpret(Statement target)
    {
        GetStrategy(target).Apply();
    }

    private StatementInterpretationStrategy GetStrategy(Statement target)
    {
        return target switch
        {
            ExpressionStatement expressionStatement => new ExpressionStatementInterpretationStrategy(expressionStatement, expressionEvaluator),
            Assignment assignment => new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator),
            Block block => HandleBlock(block),
            ForLoop forLoop => new ForLoopInterpretationStrategy(forLoop, this, declarationInterpreter, expressionEvaluator),
            WhileLoop whileLoop => new WhileLoopInterpretationStrategy(whileLoop, expressionEvaluator, this),
            _ => throw new ArgumentException()
        };
    }

    private StatementInterpretationStrategy HandleBlock(Block block)
    {
        var cloneScope = scope.CloneScope();
        cloneScope.AddNewScope();
        var statementInterpreter = new StatementInterpreter(cloneScope);
        var declarationInterpreter = new DeclarationInterpreter(cloneScope);
        return new BlockInterpretationStrategy(block, statementInterpreter, declarationInterpreter);
    }
}