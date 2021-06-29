package net.cart.action;

import java.io.PrintWriter;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import net.cart.db.CartBean;
import net.cart.db.CartDAO;
import net.action.Action;
import net.action.ActionForward;
import net.cart.db.*;

public class CartDeleteAction implements Action{
	public ActionForward execute(HttpServletRequest request,HttpServletResponse response) throws Exception{
		HttpSession session = request.getSession();
		CartDAO cartdao = new CartDAO();
		CartBean cartbean = new CartBean();
		
		cartbean.setCart_code(Integer.parseInt(request.getParameter("code")));
		
		
		if(!cartdao.deleteCart(cartbean.getCart_code())) {
				PrintWriter out = response.getWriter();
				out.println("<script>");
				out.println("alert('Deletion is failed try again')");
				out.print("</script>");
				out.close();
				return null;
		}

		ActionForward forward = new ActionForward();
		forward.setRedirect(true);

		forward.setPath("CartListAction.co");
		cartdao.conClose();
		return forward;
		
	}
}


