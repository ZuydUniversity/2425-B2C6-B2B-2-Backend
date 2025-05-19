import subprocess

def test_main_output():
    result = subprocess.run(
        ["python", "main.py"],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )
    assert "Order Order001 toegevoegd: ['Patat', 'Frikandel']" in result.stdout
    assert "Order001: ['Patat', 'Frikandel']" in result.stdout
