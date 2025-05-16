import subprocess

def test_main_output():
    result = subprocess.run(
        ["python", "main.py"],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )
    assert result.stdout.strip() == "Hello World"