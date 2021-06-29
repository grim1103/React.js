
package net.cart.action;

import java.io.PrintWriter;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import net.action.Action;
import net.action.ActionForward;
import net.cart.db.CartBean;
import net.cart.db.CartDAO;


public class CartAddAction implements Action{
	public ActionForward execute(HttpServletRequest request,HttpServletResponse response) throws Exception{
		CartDAO cartdao = new CartDAO();
		CartBean cartbean = new CartBean();
		
		cartbean.setCart_id(request.getParameter("id"));
		cartbean.setCart_code(Integer.parseInt(request.getParameter("code")));
		cartbean.setCart_image(request.getParameter("image"));
		cartbean.setCart_name(request.getParameter("name"));
		cartbean.setCart_price(Integer.parseInt(request.getParameter("price")));
		cartbean.setCart_count(Integer.parseInt(request.getParameter("count")));
		cartbean.setCart_delprice(Integer.parseInt(request.getParameter("delprice")));
		cartbean.setCart_sum(cartbean.getCart_price()*cartbean.getCart_count());
		

		if(!cartdao.insertCart(cartbean)) {
			PrintWriter out = response.getWriter();
			out.println("<script>");
			out.println("alert('Register is failed try again')");
			out.print("</script>");
			out.close();
			return null;
		}

		ActionForward forward=new ActionForward();
	    forward.setRedirect(true);
		forward.setPath("CartListAction.co");
		cartdao.conClose();
		return forward;
		
	}
}
