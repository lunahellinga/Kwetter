## Coding Standards - .net Core

The following coding standards provide guidelines for writing clean, maintainable, and consistent code in Kwetter's .NET
Core projects. Adhering to these standards will ensure code readability, improve collaboration among developers, and
enhance the overall quality of the codebase.

### Naming Conventions

- Use descriptive and meaningful names for variables, methods, classes, and namespaces.
- Follow PascalCase for class names, method names, and properties (
  e.g., `public class MyClass`, `public void MyMethod()`).
- Use camelCase for local variables and parameters (e.g., `string myVariable`, `void MyMethod(int parameter)`).
- Prefix interfaces with the letter "I" (e.g., `public interface IMyInterface`).

### Code Formatting

- Use spaces for indentation with a consistent tab width (e.g., 4 spaces).
- Use braces ({}) for block statements, even if the block contains a single line of code.
- Place opening braces on the same line as the statement, with a space before the opening brace.
- Use meaningful line breaks and indentation to improve code readability.

### Error Handling

- Use proper exception handling to handle and log errors.
- Catch specific exceptions rather than using a generic catch-all block.
- Log exceptions and provide meaningful error messages for troubleshooting.
- Avoid swallowing exceptions silently.

### Comments and Documentation

- Include XML documentation for public methods, classes, and interfaces.
- Use comments to explain complex algorithms, business rules, or any non-obvious code logic.
- Avoid unnecessary comments that state the obvious.

### Testing and Quality Assurance

- Write unit tests for critical and complex code components.
- Follow Arrange-Act-Assert (AAA) pattern in test methods.
- Aim for high code coverage to ensure robust testing.
- Use code analysis and static analysis tools to identify potential issues and maintain code quality.

### Code Organization and Structure

- Organize code files logically into folders and namespaces.
- Keep classes small, focused, and adhering to the Single Responsibility Principle (SRP).
- Use appropriate access modifiers (public, private, protected) to encapsulate functionality.
- Avoid long methods and aim for methods with a single responsibility.

### Dependency Management

- Utilize dependency injection (DI) to manage dependencies between components.
- Follow the principle of Dependency Inversion, favoring abstractions over concrete implementations.

### Source Control and Git

- Use Git for version control and follow the Git branching model (e.g., GitFlow).
- Commit frequently with meaningful commit messages.
- Review and test code changes before merging into the main branch.