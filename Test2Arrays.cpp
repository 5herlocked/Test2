#include <iostream>
#include <chrono>

void makeStackArray();
void makeStaticArray();
void makeHeapArray();

using namespace std;

int main () {
    std::chrono::time_point<std::chrono::high_resolution_clock> start, end;
    start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < 100000; i++) {
        makeStaticArray();
    }
    end = std::chrono::high_resolution_clock::now();
    auto time_taken = chrono::duration_cast<chrono::duration<double>>(end - start);
    cout << "Time to instantiate 100,000 static arrays: " << time_taken.count() << "s" << endl;

    start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < 100000; i++){
        makeStackArray();
    }
    end = std::chrono::high_resolution_clock::now();
    time_taken = chrono::duration_cast<chrono::duration<double>>(end - start);
    cout << "Time to instantiate 100,000 stack arrays: " << time_taken.count() << "s" << endl;

    start = std::chrono::high_resolution_clock::now();
    for (int i = 0; i < 100000; i++) {
        makeHeapArray();
    }
    end = std::chrono::high_resolution_clock::now();
    time_taken = chrono::duration_cast<chrono::duration<double>>(end - start);

    cout << "Time to instantiate 100,000 heap arrays: " << time_taken.count() << "s" << endl;

    return 0;
}

void makeStaticArray() {
    static int staticArray[128256];
    for (int i = 0; i < 128256; i++) {
        staticArray[i] = i;
    }
    // auto deallocation
}

void makeStackArray() {
    int stackArray[128256];
    for (int i = 0; i < 128256; i++) {
        stackArray[i] = i;
    }
}

void makeHeapArray() {
    int *heapArray = new int[128256];
    for (int i = 0; i < 128256; i++) {
        heapArray[i] = i;
    }
    delete[] heapArray;
}