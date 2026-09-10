#include <iostream>

int main() {
    std::cout << "Hello from C++ (codelldb)" << std::endl;
    int sum = 0;
    for (int i = 1; i <= 5; i++) {
        sum += i;
    }
    std::cout << "sum = " << sum << std::endl;
    return 0;
}