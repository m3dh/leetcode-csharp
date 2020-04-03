namespace LeetCode.Csharp.Solutions
{
    using System;

    public class ConcurrencySolutions
    {
        // 1115 - https://leetcode.com/problems/print-foobar-alternately/
        public class FooBar
        {
            private int n;
            private System.Threading.Semaphore _foos = new System.Threading.Semaphore(0, 1);
            private System.Threading.Semaphore _bars = new System.Threading.Semaphore(1, 1);


            public FooBar(int n)
            {
                this.n = n;

            }

            public void Foo(Action printFoo)
            {
                for (int i = 0; i < n; i++)
                {
                    this._bars.WaitOne();

                    // printFoo() outputs "foo". Do not change or remove this line.
                    printFoo();

                    this._foos.Release(1);
                }
            }

            public void Bar(Action printBar)
            {

                for (int i = 0; i < n; i++)
                {

                    this._foos.WaitOne();

                    // printBar() outputs "bar". Do not change or remove this line.
                    printBar();

                    this._bars.Release(1);
                }
            }
        }

        // 1116 - https://leetcode.com/problems/print-zero-even-odd/
        public class ZeroEvenOdd
        {
            private int n;
            private volatile int p = 0;

            private System.Threading.Semaphore _evens = new System.Threading.Semaphore(0, 1);
            private System.Threading.Semaphore _odds = new System.Threading.Semaphore(0, 1);
            private System.Threading.Semaphore _zeros = new System.Threading.Semaphore(1, 1);

            public ZeroEvenOdd(int n)
            {
                this.n = n;
            }

            // printNumber(x) outputs "x", where x is an integer.
            public void Zero(Action<int> printNumber)
            {
                while (true)
                {
                    this._zeros.WaitOne();

                    p++;

                    if (p <= n)
                    {
                        printNumber(0);

                        if (p % 2 == 1) this._odds.Release();
                        else this._evens.Release();
                    }
                    else
                    {
                        this._odds.Release();
                        this._evens.Release();
                        return;
                    }
                }
            }

            public void Even(Action<int> printNumber)
            {
                while (true)
                {
                    this._evens.WaitOne();
                    if (p > n) return;
                    printNumber(p);

                    this._zeros.Release();
                }
            }

            public void Odd(Action<int> printNumber)
            {
                while (true)
                {
                    this._odds.WaitOne();
                    if (p > n) return;
                    printNumber(p);

                    this._zeros.Release();
                }
            }
        }
    }
}