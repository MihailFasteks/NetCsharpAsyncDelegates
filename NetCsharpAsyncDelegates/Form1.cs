using System;

namespace NetCsharpAsyncDelegates
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cts;
        private long _bytesCopied;
        public Form1()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string fileToCopy = textBox1.Text;
            string whereToCopy = textBox2.Text;
            _cts = new CancellationTokenSource();
            _bytesCopied = 0;
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await ReadWriteAsync(fileToCopy, whereToCopy, _cts.Token);
                    MessageBox.Show("Копирование завершено!");
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show($"Копирование остановлено! Скопировано {_bytesCopied} байт.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка копирования: " + ex.Message);
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        }
        private async Task ReadWriteAsync(string sourcePath, string destPath, CancellationToken cancellationToken)
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            using (FileStream flSource = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true))
            using (FileStream flDest = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
            {
                long totalBytes = flSource.Length;
                Invoke(new Action(() =>
                {
                    progressBar1.Maximum = 100;
                    progressBar1.Value = 0;
                }));

                int bytesRead;
                while ((bytesRead = await flSource.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    await flDest.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                    _bytesCopied += bytesRead;
                    int progress = (int)((_bytesCopied * 100) / totalBytes);

                    Invoke(new Action(() => progressBar1.Value = progress));

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }
    }
}
