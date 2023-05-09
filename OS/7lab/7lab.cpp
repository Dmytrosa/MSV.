#include <iostream>
#include <thread>
#include <chrono>
#include <atomic>
#include <condition_variable>

using namespace std;

int f(int x) {
    while (true) {
        x /= 10;
        if (x <= 1) {
            return 0;
        }
    }
    return 1;
}

int g(int x) {
    while (true) {
        x /= 100;
        if (x <= 1) {
            return 0;
        }
    }
    return 1;
}

std::atomic<bool> stop_checking(false);
std::condition_variable cv;
std::mutex cv_m;
bool f_finished = false;
bool g_finished = false;

void execute_f(int x, int* result) {
    *result = f(x);
    f_finished = true;
    cv.notify_one();
}

void execute_g(int x, int* result) {
    *result = g(x);
    g_finished = true;
    cv.notify_one();
}

int main() {
    int x;
    std::cout << "Enter x: ";
    std::cin >> x;

    int f_result = 0;
    int g_result = 0;

    std::thread f_thread(execute_f, x, &f_result);
    std::thread g_thread(execute_g, x, &g_result);

    while (true) {
        std::this_thread::sleep_for(std::chrono::seconds(10));

        {
            std::unique_lock<std::mutex> lk(cv_m);
            cv.wait(lk, [] { return f_finished , g_finished; });
        }

        if (f_result == 0 && g_result == 0) {
            cout << "f(x) && g(x) = 0" << endl;
            stop_checking = true;
            break;
        }

        if (!stop_checking) {
            std::cout << "1 - continue calculating \n2 - stop and exit\n3 - continuå calculation without questions\n";
            int choice;
            std::cin >> choice;

            if (choice == 2) {
                stop_checking = true;
                break;
            }
            else if (choice == 3) {
                stop_checking = true;
            }
        }
    }

    if (!stop_checking) {
        std::cout << "Calculating stopped.\n";
    }

    if (f_thread.joinable()) {
        f_thread.join();
    }
    if (g_thread.joinable()) {
        g_thread.join();
    }

    return 0;
}