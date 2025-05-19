from order_system import OrderSystem

def main():
    system = OrderSystem()
    system.add_order("Order001", ["Patat", "Frikandel"])
    system.view_orders()

if __name__ == "__main__":
    main()
