fn main() {
    println!("Hello from Rust (codelldb)");
    let mut sum: i32 = 0;
    for i in 1..=5 {
        sum += i;
    }
    println!("sum = {sum}");
}