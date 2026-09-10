package main

import "fmt"

func main() {
	fmt.Println("Hello from Go (delve)")
	sum := 0
	for i := 1; i <= 5; i++ {
		sum += i
	}
	fmt.Printf("sum = %d\n", sum)
}