class OrderSystem:
    def __init__(self):
        self.orders = {}

    def add_order(self, order_id, items):
        self.orders[order_id] = items
        print(f"Order {order_id} toegevoegd: {items}")

    def view_orders(self):
        for order_id, items in self.orders.items():
            print(f"{order_id}: {items}")
